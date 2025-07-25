package com.patricksak.bptracking.exception;

import java.time.LocalDateTime;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.HttpStatusCode;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.context.request.WebRequest;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;

@ControllerAdvice
public class CustomizedResponseEntityExceptionHandler extends ResponseEntityExceptionHandler {
	
	@ExceptionHandler(Exception.class)
	public final ResponseEntity<ErrorDetails> handleAllExceptions(
			Exception ex, WebRequest request) {
		
		ErrorDetails errorDetails = new ErrorDetails(
				LocalDateTime.now(),
				ex.getMessage(),
				request.getDescription(false)
			);
		
		return new ResponseEntity<ErrorDetails>(
				errorDetails, HttpStatus.INTERNAL_SERVER_ERROR);
	}
	
	@ExceptionHandler(AccountNotFoundException.class)
	public final ResponseEntity<ErrorDetails> handleAccountNotFoundException(
			Exception ex, WebRequest request) {
		
		ErrorDetails errorDetails = new ErrorDetails(
				LocalDateTime.now(),
				ex.getMessage(),
				request.getDescription(false)
			);
		
		return new ResponseEntity<ErrorDetails>(
				errorDetails, HttpStatus.NOT_FOUND);
	}
	
	@ExceptionHandler(BPDataNotFoundException.class)
	public final ResponseEntity<ErrorDetails> handleDataNotFoundException(
			Exception ex, WebRequest request) {
		
		ErrorDetails errorDetails = new ErrorDetails(
				LocalDateTime.now(),
				ex.getMessage(),
				request.getDescription(false)
			);
		
		return new ResponseEntity<ErrorDetails>(
				errorDetails, HttpStatus.NOT_FOUND);
	}
	
	@Override
	protected ResponseEntity<Object> handleMethodArgumentNotValid(
			MethodArgumentNotValidException ex, HttpHeaders headers,
			HttpStatusCode status, WebRequest request) {
		
		String errorMessage = String.format("Total errors: %s - %s",
				ex.getErrorCount(), 
				ex.getFieldError().getDefaultMessage());
		
		ErrorDetails errorDetails = new ErrorDetails(
				LocalDateTime.now(),
				errorMessage,
				request.getDescription(false)
			);
		
		return new ResponseEntity<Object>(
				errorDetails, HttpStatus.BAD_REQUEST);
	}

}