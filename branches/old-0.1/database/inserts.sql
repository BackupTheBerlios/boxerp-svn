
-- Table: TABLES 
insert into tables values(0,'ctrld_actions','Users actions');
insert into tables values(1,'ctrld_sections','Actions happends over sections');
insert into tables values(2,'permissions','Every group has actions over sections');
insert into tables values(3,'users','Users');
insert into tables values(4,'groups','Groups');
insert into tables values(5,'enterprises','Enterprises');                                                              
insert into tables values(6,'tables','Contains all database table names');
insert into tables values(7,'error_codes','Error Codes');

-- Table: ERROR_CODES
insert into error_codes values(0,'Login incorrect');

-- Table: CTRLD_ACTIONS
insert into ctrld_actions values (0,'query','Query or see data');
insert into ctrld_actions values (1,'create','Create or add new elements');
insert into ctrld_actions values (2,'modify','Modify existent data');
insert into ctrld_actions values (3,'execute','Process execution');
insert into ctrld_actions values (4,'delete','Delete items');
insert into ctrld_actions values (5,'showui','Show widget/s from interface');

-- Table: CTRLD_SECTIONS
insert into ctrld_sections values (0,'admin', 'Administration');
insert into ctrld_sections values (1,'adminusers','Users administracion');
insert into ctrld_sections values (2,'admingroups','Groups administracion');
insert into ctrld_sections values (3,'adminenterprises','Enterprises administracion');
insert into ctrld_sections values (4,'sales', 'Sales');
insert into ctrld_sections values (5,'purchases', 'Purchases');
insert into ctrld_sections values (6,'accouting', 'Accounting');
insert into ctrld_sections values (7,'packing', 'Packing');
insert into ctrld_sections values (8,'storehouse', 'Storehouse');
insert into ctrld_sections values (9,'office', 'Office');
insert into ctrld_sections values (10,'management','Management');

-- Table: ENTERPRISES
insert into enterprises values (0, 'Pruebas', true, 'Empresa para pruebas');

-- Table: GROUPS
insert into groups values (0, 0, 'Skelgroup', true);

-- Table: USERS
insert into users values (0, 0, 'demo','demo',true);

-- Table: PERMISSIONS
insert into permissions values (0, 0 , 0);
insert into permissions values (0, 1 , 0);
insert into permissions values (0, 2 , 0);
insert into permissions values (0, 3 , 0);
insert into permissions values (0, 0 , 1);
insert into permissions values (0, 1 , 1);
insert into permissions values (0, 2 , 1);
insert into permissions values (0, 3 , 1);
insert into permissions values (0, 0 , 2);
insert into permissions values (0, 1 , 2);
insert into permissions values (0, 2 , 2);
insert into permissions values (0, 3 , 2);
insert into permissions values (0, 4, 0);
insert into permissions values (0, 4, 1);
insert into permissions values (0, 4, 2);
insert into permissions values (0, 4, 3);
insert into permissions values (0, 5, 0);
insert into permissions values (0, 5, 1);
insert into permissions values (0, 5, 2);
insert into permissions values (0, 5, 3);



