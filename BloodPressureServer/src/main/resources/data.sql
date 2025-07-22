-- Password: 1234
INSERT INTO Account (id, email, password, nickname, birthdate, description, role, active)
VALUES ('410c2d4e-c7b2-4088-9ebb-12087c527ba7', 'test_email1@hotmail.com', '$2a$12$5D8hxDtdKQKjkQ.8rqmkGevtemwyYYGTU4X6RM0jGyMyMrfOpAqE.', 'User1', '2001-1-1', 'Hello!', 'USER', true);

-- Password: 1234
INSERT INTO Account (id, email, password, nickname, birthdate, description, role, active)
VALUES ('4fe1acc9-f533-4bf8-a20c-37b7c3c70050', 'test_email2@hotmail.com', '$2a$12$5D8hxDtdKQKjkQ.8rqmkGevtemwyYYGTU4X6RM0jGyMyMrfOpAqE.', 'User2', '2001-1-1', 'Hello!', 'ADMIN,USER', true);

INSERT INTO BPData (account_id, systolic, diastolic, heart_rate, sugar_level, post_time)
VALUES ('410c2d4e-c7b2-4088-9ebb-12087c527ba7', 50, 50, 50, 50, '2024-10-13T17:54:38.721Z');

INSERT INTO BPData (account_id, systolic, diastolic, heart_rate, sugar_level, post_time)
VALUES ('410c2d4e-c7b2-4088-9ebb-12087c527ba7', 60, 60, 60, 60, '2024-10-14T17:54:38.721Z');

INSERT INTO BPData (account_id, systolic, diastolic, heart_rate, sugar_level, post_time)
VALUES ('4fe1acc9-f533-4bf8-a20c-37b7c3c70050', 70, 70, 70, 70, '2024-10-15T17:54:38.721Z');

INSERT INTO BPData (account_id, systolic, diastolic, heart_rate, sugar_level, post_time)
VALUES ('4fe1acc9-f533-4bf8-a20c-37b7c3c70050', 80, 80, 80, 80, '2024-10-16T17:54:38.721Z');