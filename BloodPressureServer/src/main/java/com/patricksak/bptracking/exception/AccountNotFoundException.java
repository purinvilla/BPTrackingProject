package com.patricksak.bptracking.exception;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(code = HttpStatus.NOT_FOUND)
public class AccountNotFoundException extends RuntimeException {
	
	private static final long serialVersionUID = -49755635140403025L;

	public AccountNotFoundException(String message) {
		super(message);
	}

}
