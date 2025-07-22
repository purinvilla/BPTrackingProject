package com.patricksak.bptracking.bpdata;

import java.time.LocalDateTime;
import java.util.UUID;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

@Entity
public class BPData {

	// Properties
	@Id
	@GeneratedValue( strategy = GenerationType.IDENTITY )
	@JsonProperty("Id")
	private long id;
	
	@JsonProperty("Accountid")
	private UUID accountId;
	
	@JsonFormat(pattern = "yyyy-MM-dd HH:mm:ss")
	@JsonProperty("Posttime")
	private LocalDateTime postTime;
	
	@JsonProperty("Systolic")
	private int systolic;
	@JsonProperty("Diastolic")
	private int diastolic;
	@JsonProperty("Heartrate")
	private int heartRate;
	@JsonProperty("Sugarlevel")
	private int sugarLevel;

	// Getters and setters
	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public UUID getAccountId() {
		return accountId;
	}

	public void setAccountId(UUID accountId) {
		this.accountId = accountId;
	}

	public LocalDateTime getPostTime() {
		return postTime;
	}

	public void setPostTime(LocalDateTime postTime) {
		this.postTime = postTime;
	}

	public int getSystolic() {
		return systolic;
	}

	public void setSystolic(int systolic) {
		this.systolic = systolic;
	}

	public int getDiastolic() {
		return diastolic;
	}

	public void setDiastolic(int diastolic) {
		this.diastolic = diastolic;
	}

	public int getHeartRate() {
		return heartRate;
	}

	public void setHeartRate(int heartRate) {
		this.heartRate = heartRate;
	}

	public int getSugarLevel() {
		return sugarLevel;
	}

	public void setSugarLevel(int sugarLevel) {
		this.sugarLevel = sugarLevel;
	}

	// toString
	@Override
	public String toString() {
		return String.format(
				"BPData [id=%s, accountId=%s, postTime=%s, systolic=%s, diastolic=%s, heartRate=%s, sugarLevel=%s]", id,
				accountId, postTime, systolic, diastolic, heartRate, sugarLevel);
	}
	
}
