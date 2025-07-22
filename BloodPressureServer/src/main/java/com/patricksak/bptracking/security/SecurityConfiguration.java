package com.patricksak.bptracking.security;

import static org.springframework.security.config.Customizer.withDefaults;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.ProviderManager;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityCustomizer;
import org.springframework.security.config.annotation.web.configurers.AbstractHttpConfigurer;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.security.web.util.matcher.AntPathRequestMatcher;

import com.patricksak.bptracking.services.AccountDetailsService;

@Configuration
@EnableWebSecurity
public class SecurityConfiguration {
	
	// Properties
	@Autowired
	private AccountDetailsService accountDetailsService;
	@Autowired
	private JwtAuthenticationFilter jwtAuthenticationFilter;
	
	// Methods
	@Bean
	public SecurityFilterChain securityFilterChain(HttpSecurity httpSecurity) throws Exception {
		httpSecurity		
		.authorizeHttpRequests(auth -> {
			auth.requestMatchers("/register/**","/authenticate").permitAll();
			auth.requestMatchers("/admin/**").hasRole("ADMIN");
			auth.requestMatchers("/accounts/**").hasRole("USER");
			auth.anyRequest().authenticated();
		});
		
		// Use basic authentication. Good for RestAPI
		httpSecurity.httpBasic(withDefaults());
		
		// Use login page to authenticate. Good for web app		
//		httpSecurity.formLogin(formLogin -> formLogin.permitAll());

		httpSecurity.csrf(AbstractHttpConfigurer::disable);
		httpSecurity.addFilterBefore(jwtAuthenticationFilter, UsernamePasswordAuthenticationFilter.class);
		return httpSecurity.build();
	}

	// This part will make h2-console work un the spring boot security.
	// Disable it when in production.
	@Bean
    public WebSecurityCustomizer webSecurityCustomizer() {
        return (web) -> web.ignoring().requestMatchers(new AntPathRequestMatcher("/h2-console/**"));
    }
	
//	@Bean
//	public UserDetailsService userDetailService() {
//		UserDetails normalUser = User.builder()
//				.username("gc")
//				.password("$2a$12$JHwHTjdo0vNV8UHl6zsAGudLCH8rsXw3FcQgVwS8LtO4AKdEmyNqa")
//				.roles("USER")
//				.build();
//		
//		UserDetails adminUser = User.builder()
//				.username("admin")
//				.password("$2a$12$p5CFgVvQc6B56SLM9ESAOeEHYtkRzj7T2CwujSH04MjOVh5n5DXF2")
//				.roles("ADMIN", "USER")
//				.build();
//		
//		return new InMemoryUserDetailsManager(normalUser, adminUser);
//	}
	
	@Bean
	public UserDetailsService userDetailsService() {
		return accountDetailsService;
	}
	
	@Bean
	public AuthenticationProvider authenticationProvider() {
		DaoAuthenticationProvider provider = new DaoAuthenticationProvider();
		
		provider.setUserDetailsService(accountDetailsService);
		provider.setPasswordEncoder(passwordEncoder());
		
		return provider;
	}
	
	@Bean
	public AuthenticationManager authenticationManager() {
		return new ProviderManager(authenticationProvider());
	}
	
	@Bean
	public PasswordEncoder passwordEncoder() {
		return new BCryptPasswordEncoder();
	}

}
