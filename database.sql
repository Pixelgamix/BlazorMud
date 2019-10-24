CREATE TABLE "public"."account" (
    "account_id" uuid NOT NULL,
    "account_name" character varying(32) NOT NULL,
    "hashed_password" character(48) NOT NULL,
    "created_at" timestamp DEFAULT LOCALTIMESTAMP NOT NULL,
    "last_login" timestamp,
    CONSTRAINT "Account_AccountId" PRIMARY KEY ("account_id"),
    CONSTRAINT "Account_AccountName" UNIQUE ("account_name")
) WITH (oids = false);


CREATE TABLE "public"."playercharacter" (
    "account_id" uuid NOT NULL,
    "playercharacter_id" uuid NOT NULL,
    "forename" character varying(12) NOT NULL,
    "surname" character varying(12) NOT NULL,
    "created_at" timestamp NOT NULL,
    "last_selected" timestamp NOT NULL,
    CONSTRAINT "playercharacter_forename_surname" UNIQUE ("forename", "surname"),
    CONSTRAINT "playercharacter_playercharacter_id" PRIMARY KEY ("playercharacter_id"),
    CONSTRAINT "playercharacter_account_id_fkey" FOREIGN KEY (account_id) REFERENCES account(account_id) NOT DEFERRABLE
) WITH (oids = false);