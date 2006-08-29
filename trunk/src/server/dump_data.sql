COPY sgroups (id, groupname, published) FROM stdin;
0	demogroup	t
\.


COPY susers (id, username, "password", published) FROM stdin;
0	demo	pass	t
\.


