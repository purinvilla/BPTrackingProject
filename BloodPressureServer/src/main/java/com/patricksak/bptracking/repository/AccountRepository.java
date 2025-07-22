package com.patricksak.bptracking.repository;

import java.util.UUID;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.patricksak.bptracking.account.Account;

public interface AccountRepository extends JpaRepository<Account, UUID> {

	@Query("SELECT a FROM Account a WHERE a.email = :email")
	Account findByEmail(@Param("email") String email);
	
}
