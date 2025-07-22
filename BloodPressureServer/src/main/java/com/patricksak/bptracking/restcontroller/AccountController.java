package com.patricksak.bptracking.restcontroller;

import java.net.URI;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import com.patricksak.bptracking.account.Account;
import com.patricksak.bptracking.bpdata.BPData;
import com.patricksak.bptracking.exception.AccountNotFoundException;
import com.patricksak.bptracking.exception.BPDataNotFoundException;
import com.patricksak.bptracking.repository.AccountRepository;
import com.patricksak.bptracking.repository.BPDataRepository;
import com.patricksak.bptracking.services.AccountDetailsService;
import com.patricksak.bptracking.services.JwtService;

@RestController
public class AccountController {
	
	// Properties
	@Autowired
	private AccountRepository accountRepository;
	@Autowired
	private BPDataRepository dataRepository;
	@Autowired
	private PasswordEncoder passwordEncoder;
	@Autowired
	private AuthenticationManager authenticationManager;
	@Autowired
	private JwtService jwtService;
	@Autowired
	private AccountDetailsService accountDetailsService;
	
	// Constructor
	public AccountController(AccountRepository accountRepository, BPDataRepository dataRepository) {
		this.accountRepository = accountRepository;
		this.dataRepository = dataRepository;
	}
	
	// Checks for account authentication
	private UUID getAuthUUID() {
		Authentication auth = 	SecurityContextHolder.getContext().getAuthentication();
		String userEmail = auth.getName();
		UUID accountId = accountRepository.findByEmail(userEmail).getId();
		return accountId;
	}
	
	// Get token for authentication
	@PostMapping("/authenticate")
	public String authenticateAndGetToken(@RequestBody LoginForm loginForm) {
		Authentication authentication = authenticationManager.authenticate(
				new UsernamePasswordAuthenticationToken(loginForm.getUsername(),loginForm.getPassword()));
		if(authentication.isAuthenticated()) {
			return jwtService.generateToken(accountDetailsService.loadUserByUsername(loginForm.getUsername()));
		} else {
			throw new UsernameNotFoundException("Invalid credentials");
		}
	}
	
	/*
	 * [ACCESSABLE PAGES]
	 * 
	 * PUBLICALLY AVAILABLE:
	 * - POST /register/user
	 * 
	 * FOR USERS:
	 * - GET /account
	 * - PUT /account
	 * - DELETE /account
	 * - GET /account/data
	 * - POST /account/data
	 * - PUT /account/data/{id}
	 * - DELETE /account/data/{id}
	 * 
	 * FOR ADMINS:
	 * - GET /admin/accounts
	 * - GET /admin/accounts/{id}
	 * - POST /admin/accounts
	 * - PUT /admin/accounts/{id}
	 * - DELETE /admin/accounts/{id}
	 * - GET /admin/data
	 * - GET /admin/accounts/{id}/data
	 * - POST /admin/accounts/{id}/data
	 * - PUT /admin/accounts/{id}/data/{id}
	 * - DELETE /admin/data/{id}
	 * */
	
	/*
	 * PUBLICALLY AVAILABLE
	 */
	
	// POST /accounts - Add new user
	@PostMapping("/register/user")
	public ResponseEntity<Account> registerAccount(@RequestBody Account account) {
		// TODO: Need to verify email before creating new account; for now, activate everyone
		account.setPassword(
				passwordEncoder.encode(account.getPassword()));
		account.setRole("USER");
		account.setActive(true);
		
		Account savedAccount = accountRepository.save(account);
		
		// Send HTML response
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/accounts/{id}")
				.buildAndExpand(savedAccount.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	/*
	 * FOR REGULAR USERS
	 */
	
	// GET /account - Logged-in user
	@GetMapping("/account")
	public Optional<Account> retrieveUserAccount() {
		UUID id = getAuthUUID();
		Optional<Account> account = accountRepository.findById(id);
		return account;
	}
	
	// PUT /accounts - Logged-in user
	@PutMapping("/account")
	public ResponseEntity<?> updateUserAccount(@RequestBody Account updatedAccount) {
		UUID id = getAuthUUID();
		
		// Exception handlers
		if(accountRepository.findById(id).isEmpty())
			throw new AccountNotFoundException("User ID not found: " + id);
		
		// Get and update account
		Account savedAccount = accountRepository.findById(id)
				.map(account -> {
					account.setEmail(updatedAccount.getEmail());
					account.setPassword(updatedAccount.getPassword());
					account.setNickname(updatedAccount.getNickname());
					account.setBirthdate(updatedAccount.getBirthdate());
					account.setDescription(updatedAccount.getDescription());
					return accountRepository.save(account);
				})
				.orElseGet(() -> {
					updatedAccount.setId(id);
					return accountRepository.save(updatedAccount);
				});
		
		// Get URI of post
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("accounts/{id}")
				.buildAndExpand(savedAccount.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// DELETE /account - Logged-in user
	@DeleteMapping("/account")
	public void deleteUserAccount() {
		UUID id = getAuthUUID();
		accountRepository.deleteById(id);
	}
	
	// GET /data - Logged-in user
	@GetMapping("/account/data")
	public List<BPData> retrieveUserData() {
		UUID id = getAuthUUID();
		return dataRepository.findByAccountId(id);
	}
	
	// GET /data - Logged-in user, specific date
	@GetMapping("/account/data/date/{beginDate}")
	public List<BPData> retrieveUserDataFromDate(@PathVariable String beginDate) {
		UUID id = getAuthUUID();
		
		//Create a DateTimeFormatter with your required format:
		//Note: https://docs.oracle.com/javase/8/docs/api/java/time/format/DateTimeFormatter.html
	    DateTimeFormatter dateTimeFormat = DateTimeFormatter.ISO_LOCAL_DATE_TIME;
	    
	    //Next parse the date from the @PathVariable
	    LocalDateTime bDate = dateTimeFormat.parse(beginDate+"T00:00:00", LocalDateTime::from);
	    //Set endtime to 1 day
	    LocalDateTime eDate = dateTimeFormat.parse(beginDate+"T23:59:59", LocalDateTime::from);
	   
		return dataRepository.findByAccountIdbeginDate(id, bDate, eDate);
	}
	
	// GET /data - Logged-in user, between two dates
	@GetMapping("/account/data/date/{beginDate}/{endDate}")
	public List<BPData> retrieveUserDataFromPeriod(@PathVariable String beginDate, @PathVariable String endDate) {
		UUID id = getAuthUUID();
		
		//Create a DateTimeFormatter with your required format:
		//Note: https://docs.oracle.com/javase/8/docs/api/java/time/format/DateTimeFormatter.html
	    DateTimeFormatter dateTimeFormat = DateTimeFormatter.ISO_LOCAL_DATE_TIME;
	    
	    //Next parse dates from the @PathVariable
	    LocalDateTime bDate = dateTimeFormat.parse(beginDate + "T00:00:00", LocalDateTime::from);
	    LocalDateTime eDate = dateTimeFormat.parse(endDate + "T23:59:59", LocalDateTime::from);
	   
		return dataRepository.findByAccountIdbeginDate(id, bDate, eDate);
	}
	
	// POST /data - Logged-in user, single data
	@PostMapping("/account/data")
	public ResponseEntity<BPData> addUserData(@RequestBody BPData bpData) {
		UUID id = getAuthUUID();
		
		bpData.setPostTime(LocalDateTime.now());
		bpData.setAccountId(id);
		BPData savedBPData = dataRepository.save(bpData);
		
		// Send HTML response
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(savedBPData.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// PUT /data - Logged-in user, single data
	@PutMapping("/account/data/{dataId}")
	public ResponseEntity<?> updateUserData(@PathVariable long dataId, @RequestBody BPData updatedData) {
		// Get account
		UUID accountId = getAuthUUID();
		
		// Exception handlers
		if(dataRepository.findById(dataId).isEmpty())
			throw new BPDataNotFoundException("Entry ID not found: " + dataId);
		if(dataRepository.findById(dataId).get().getAccountId().compareTo(accountId) != 0)
			throw new AccountNotFoundException("User (" + accountId + ") does not own this post.");
		
		// Get data entry and update contents
		BPData savedData = dataRepository.findById(dataId)
				.map(data -> {
					if (updatedData.getPostTime() !=null) data.setPostTime(updatedData.getPostTime());
					data.setSystolic(updatedData.getSystolic());
					data.setDiastolic(updatedData.getDiastolic());
					data.setHeartRate(updatedData.getHeartRate());
					data.setSugarLevel(updatedData.getSugarLevel());
					return dataRepository.save(data);
				})
				.orElseGet(() -> {
					updatedData.setId(dataId);
					return dataRepository.save(updatedData);
				});
		
		// Get URI of post
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(savedData.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}

	
	// DELETE /data - Logged-in user, single data
	@DeleteMapping("/account/data/{dataId}")
	public void deleteUserDataById(@PathVariable long dataId) {
		UUID accountId = getAuthUUID();
		Optional<BPData> savedData = dataRepository.findById(dataId);
		
		// Exception handlers
		if(savedData.get().getAccountId().compareTo(accountId) != 0)
			throw new AccountNotFoundException("User (" + accountId + ") does not own this post.");
		
		dataRepository.deleteById(dataId);
	}
	
	/*
	 * FOR ADMINISTRATORS ONLY
	 */
	
	// GET /accounts - All users
	@GetMapping("/admin/accounts")
	public List<Account> retrieveAllAccounts() {
		return accountRepository.findAll();
	}
	
	// GET /accounts - Specific user by id
	@GetMapping("/admin/accounts/{id}")
	public Optional<Account> retrieveAccountById(@PathVariable UUID id) {
		Optional<Account> account = accountRepository.findById(id);
		return account;
	}
	
	// GET /accounts - Specific user from email
	@GetMapping("/admin/accounts/email/{email}")
	public Account retrieveAccountByEmail(@PathVariable String email) {
		Account account = accountRepository.findByEmail(email);
		return account;
	}
	
	// POST /accounts - Single user
	@PostMapping("/admin/accounts")
	public ResponseEntity<BPData> addDataForAccount(@RequestBody Account account) {
		account.setLastSeen(LocalDateTime.now());
		Account newAccount = accountRepository.save(account);
		
		// Send HTML response
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(newAccount.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// PUT /accounts - Specific user by id
	@PutMapping("/admin/accounts/{id}")
	public ResponseEntity<?> updateAccount(@PathVariable UUID id, @RequestBody Account updatedAccount) {
		// Exception handlers
		if(accountRepository.findById(id).isEmpty())
			throw new AccountNotFoundException("User ID not found: " + id);
		
		// Get and update account
		Account savedAccount = accountRepository.findById(id)
				.map(account -> {
					account.setEmail(updatedAccount.getEmail());
					account.setPassword(updatedAccount.getPassword());
					account.setNickname(updatedAccount.getNickname());
					account.setBirthdate(updatedAccount.getBirthdate());
					account.setDescription(updatedAccount.getDescription());
					return accountRepository.save(account);
				})
				.orElseGet(() -> {
					updatedAccount.setId(id);
					return accountRepository.save(updatedAccount);
				});
		
		// Get URI of post
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(savedAccount.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// DELETE /accounts - Single user by id
	@DeleteMapping("/admin/accounts/{id}")
	public void deleteUser(@PathVariable UUID id) {
		accountRepository.deleteById(id);
	}
	
	// GET /data - All data
	@GetMapping("/admin/data")
	public List<BPData> retrieveAllData() {
		return dataRepository.findAll();
	}
	
	// GET /data - Single user by id
	@GetMapping("/admin/accounts/{id}/data")
	public List<BPData> retrieveDataById(@PathVariable UUID id) {
		return dataRepository.findByAccountId(id);
	}
	
	// POST /data - Single user, single data
	@PostMapping("/admin/accounts/{id}/data")
	public ResponseEntity<BPData> addDataForAccount(@PathVariable UUID id, @RequestBody BPData bpData) {
		bpData.setPostTime(LocalDateTime.now());
		bpData.setAccountId(id);
		BPData savedBPData = dataRepository.save(bpData);
		
		// Send HTML response
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(savedBPData.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// PUT /data - Single user, single data
	@PutMapping("/admin/accounts/{accountId}/data/{dataId}")
	public ResponseEntity<?> updateDataForAccount(@PathVariable UUID accountId,
			@PathVariable long dataId, @RequestBody BPData updatedData) {
		// Get account
		Optional<Account> account = accountRepository.findById(accountId);
		
		// Exception handlers
		if(account.isEmpty())
			throw new AccountNotFoundException("User ID not found: " + accountId);
		if(dataRepository.findById(dataId).isEmpty())
			throw new BPDataNotFoundException("Entry ID not found: " + dataId);
		if(dataRepository.findById(dataId).get().getAccountId().compareTo(account.get().getId()) != 0)
			throw new AccountNotFoundException(
					"User (" + accountId + ") does not own this post.");
		
		// Get data entry and update contents
		BPData savedData = dataRepository.findById(dataId)
				.map(data -> {
					if (updatedData.getPostTime() !=null) data.setPostTime(updatedData.getPostTime());
					data.setSystolic(updatedData.getSystolic());
					data.setDiastolic(updatedData.getDiastolic());
					data.setHeartRate(updatedData.getHeartRate());
					data.setSugarLevel(updatedData.getSugarLevel());
					return dataRepository.save(data);
				})
				.orElseGet(() -> {
					updatedData.setId(dataId);
					return dataRepository.save(updatedData);
				});
		
		// Get URI of post
		URI location = ServletUriComponentsBuilder.fromCurrentRequest()
				.path("/data/{id}")
				.buildAndExpand(savedData.getId())
				.toUri();
		
		return ResponseEntity.created(location).build();
	}
	
	// DELETE /data - Single data
	@DeleteMapping("/admin/data/{id}")
	public void deleteUser(@PathVariable long id) {
		dataRepository.deleteById(id);
	}
	
}
