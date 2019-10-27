CREATE TABLE IF NOT EXISTS originsvc.customer
(
    id SERIAL PRIMARY KEY,
    first_name varchar(250) NOT NULL,
    last_name varchar(250) NOT NULL,
    email varchar(250),
    CONSTRAINT email_unique_constraint UNIQUE (email)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE originsvc.customer
    OWNER to postgres;