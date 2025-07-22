package com.patricksak.bptracking.account;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.UUID;

import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class Account {
	
	// Properties
	@Id
	@GeneratedValue( strategy = GenerationType.UUID )
	@JsonProperty("Id")
	private UUID id;
	
	@Column(unique=true)
	@JsonProperty("Email")
	private String email;

	@JsonProperty("Password")
	private String password;
	
	@JsonProperty("Nickname")
	private String nickname;
	
	@JsonProperty("Birthdate")
	private LocalDate birthdate;

	@JsonProperty("Description")
	private String description;

	@JsonProperty("Lastseen")
	private LocalDateTime lastSeen;

	@JsonProperty("Role")
	private String role;		// e.g. ADMIN,USER

	@JsonProperty("Active")
	private boolean active;
	
	// Getter and setters
	public UUID getId() {
		return id;
	}

	public void setId(UUID id) {
		this.id = id;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getNickname() {
		return nickname;
	}

	public void setNickname(String nickname) {
		this.nickname = nickname;
	}

	public LocalDate getBirthdate() {
		return birthdate;
	}

	public void setBirthdate(LocalDate birthdate) {
		this.birthdate = birthdate;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public LocalDateTime getLastSeen() {
		return lastSeen;
	}

	public void setLastSeen(LocalDateTime lastSeen) {
		this.lastSeen = lastSeen;
	}

	public String getRole() {
		return role;
	}

	public void setRole(String role) {
		this.role = role;
	}

	public boolean isActive() {
		return active;
	}

	public void setActive(boolean active) {
		this.active = active;
	}

	// toString
	@Override
	public String toString() {
		return String.format(
				"Account [id=%s, email=%s, password=%s, nickname=%s, birthdate=%s, description=%s, lastseen=%s, role=%s]",
				id, email, password, nickname, birthdate, description, lastSeen, role);
	}
	
}
