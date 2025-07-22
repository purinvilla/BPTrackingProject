package com.patricksak.bptracking.exception;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(code = HttpStatus.NOT_FOUND)
public class BPDataNotFoundException extends RuntimeException {
	
	private static final long serialVersionUID = 2231369391245678786L;

	public BPDataNotFoundException(String message) {
		super(message);
	}

}
