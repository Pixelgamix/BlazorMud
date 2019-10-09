CREATE TABLE "public"."account" (
    "account_id" uuid NOT NULL,
    "account_name" character varying(32) NOT NULL,
    "hashed_password" character(48) NOT NULL,
    "created_at" timestamp DEFAULT LOCALTIMESTAMP NOT NULL,
    "last_login" timestamp,
    CONSTRAINT "Account_AccountId" PRIMARY KEY ("account_id"),
    CONSTRAINT "Account_AccountName" UNIQUE ("account_name")
) WITH (oids = false);