package com.patricksak.bptracking.services;

import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import com.patricksak.bptracking.account.Account;
import com.patricksak.bptracking.repository.AccountRepository;

@Service
public class AccountDetailsService implements UserDetailsService {

	// Properties
	@Autowired
	private AccountRepository repository;
	
	// Methods
	// Load an account through its assigned email
	@Override
	public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException {
		Optional<Account> account = Optional.ofNullable(repository.findByEmail(email));
		
		if(account.isPresent()) {
			var accountObj = account.get();
			return User.builder()
					.username(accountObj.getEmail())
					.password(accountObj.getPassword())
					.roles(getRoles(accountObj))
					.build();
		} else {
			throw new UsernameNotFoundException(email);
		}
	}

	// Get roles attached to an account
	private String[] getRoles(Account account) {
		if(account.getRole() == null) {
			return new String[]{"USER"};
		}
		
		return account.getRole().split(",");
	}

}
