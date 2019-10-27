INSERT INTO originsvc.customer(first_name, last_name, email)
VALUES
    ('Paul', 'Jackob', 'paul@example.org'),
    ('Mark', 'Anderson', 'mark@example.org'),
    ('Joe', 'Andrews', 'joe@example.org')
ON CONFLICT (email) DO NOTHING;