package com.patricksak.bptracking.exception;

import java.time.LocalDateTime;

public class ErrorDetails {
	
	// Properties
	private LocalDateTime timestamp;
	private String message;
	private String details;
	
	// Constructor
	public ErrorDetails(LocalDateTime timestamp, String message, String details) {
		super();
		this.timestamp = timestamp;
		this.message = message;
		this.details = details;
	}

	// Getters
	public LocalDateTime getTimestamp() {
		return timestamp;
	}

	public String getMessage() {
		return message;
	}

	public String getDetails() {
		return details;
	}

}