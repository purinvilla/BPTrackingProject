package com.patricksak.bptracking.repository;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.patricksak.bptracking.bpdata.BPData;

public interface BPDataRepository extends JpaRepository<BPData, Long> {

	// **********************************************************
	// Note!!! Fields used in query has to match Java field names
	// **********************************************************

	@Query("SELECT a FROM BPData a WHERE a.accountId = :id ORDER BY a.postTime DESC")
	List<BPData> findByAccountId(@Param("id") UUID id);
	
	@Query("SELECT a FROM BPData a WHERE a.accountId = :id AND a.postTime between :beginDate AND :endDate ORDER BY a.postTime DESC")
	List<BPData> findByAccountIdbeginDate(@Param("id") UUID id,@Param("beginDate") LocalDateTime beginDate,@Param("endDate") LocalDateTime endDate);
	

}
