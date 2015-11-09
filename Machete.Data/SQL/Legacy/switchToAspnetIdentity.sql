--This is the script for migrating from the SQL Membership tables (aspnetdb%) to
--the ASP.NET Identity tables. For creating new database objects, see the other
--script at Machete.Data/SQL/201312300649350_MergeMacheteContextAndIdentityDbContext.sql

use [machete-somedbname];
go

IF OBJECT_ID('AspNetUserRoles', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserRoles;
END

IF OBJECT_ID('AspNetUserClaims', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserClaims;
END

IF OBJECT_ID('AspNetUserLogins', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserLogins;
END

IF OBJECT_ID('AspNetRoles', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetRoles;
END

IF OBJECT_ID('AspNetUsers', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUsers; 
END

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]            NVARCHAR (128) NOT NULL,
    [UserName]      NVARCHAR (MAX) NULL,
    [PasswordHash]  NVARCHAR (MAX) NULL,
    [SecurityStamp] NVARCHAR (MAX) NULL,
    [Discriminator] NVARCHAR (128) NOT NULL,
    [ApplicationId]                          UNIQUEIDENTIFIER NOT NULL,
    [LegacyPasswordHash]  NVARCHAR (MAX) NULL,
    [LoweredUserName]  NVARCHAR (256)   NOT NULL,
    [MobileAlias]      NVARCHAR (16)    DEFAULT (NULL) NULL,
    [IsAnonymous]      BIT              DEFAULT ((0)) NOT NULL,
    [LastActivityDate] DATETIME2         NOT NULL,
    [MobilePIN]                              NVARCHAR (16)    NULL,
    [Email]                                  NVARCHAR (256)   NULL,
    [LoweredEmail]                           NVARCHAR (256)   NULL,
    [PasswordQuestion]                       NVARCHAR (256)   NULL,
    [PasswordAnswer]                         NVARCHAR (128)   NULL,
    [IsApproved]                             BIT              NOT NULL,
    [IsLockedOut]                            BIT              NOT NULL,
    [CreateDate]                             DATETIME2                 NOT NULL,
    [LastLoginDate]                          DATETIME2         NOT NULL,
    [LastPasswordChangedDate]                DATETIME2         NOT NULL,
    [LastLockoutDate]                        DATETIME2         NOT NULL,
    [FailedPasswordAttemptCount]             INT              NOT NULL,
    [FailedPasswordAttemptWindowStart]       DATETIME2         NOT NULL,
    [FailedPasswordAnswerAttemptCount]       INT              NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] DATETIME2         NOT NULL,
    [Comment]                                NTEXT            NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    --FOREIGN KEY ([ApplicationId]) REFERENCES [aspnetdb-somedbname].[dbo].[aspnet_Applications] ([ApplicationId]),
);

INSERT INTO AspNetUsers(Id,UserName,PasswordHash,Discriminator,SecurityStamp,
ApplicationId,LoweredUserName,MobileAlias,IsAnonymous,LastActivityDate,LegacyPasswordHash,
MobilePIN,Email,LoweredEmail,PasswordQuestion,PasswordAnswer,IsApproved,IsLockedOut,CreateDate,
LastLoginDate,LastPasswordChangedDate,LastLockoutDate,FailedPasswordAttemptCount,
FailedPasswordAnswerAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAttemptWindowStart,Comment)
SELECT aspnet_Users.UserId,aspnet_Users.UserName,(aspnet_Membership.Password+'|'+CAST(aspnet_Membership.PasswordFormat as varchar)+'|'+aspnet_Membership.PasswordSalt),'ApplicationUser',NewID(),aspnet_Users.ApplicationId,aspnet_Users.LoweredUserName,
aspnet_Users.MobileAlias,aspnet_Users.IsAnonymous,aspnet_Users.LastActivityDate,aspnet_Membership.Password,
aspnet_Membership.MobilePIN,aspnet_Membership.Email,aspnet_Membership.LoweredEmail,aspnet_Membership.PasswordQuestion,aspnet_Membership.PasswordAnswer,
aspnet_Membership.IsApproved,aspnet_Membership.IsLockedOut,aspnet_Membership.CreateDate,aspnet_Membership.LastLoginDate,aspnet_Membership.LastPasswordChangedDate,
aspnet_Membership.LastLockoutDate,aspnet_Membership.FailedPasswordAttemptCount, aspnet_Membership.FailedPasswordAnswerAttemptWindowStart,
aspnet_Membership.FailedPasswordAnswerAttemptCount,aspnet_Membership.FailedPasswordAttemptWindowStart,aspnet_Membership.Comment
FROM  [aspnetdb-somedbname].[dbo].aspnet_Users
LEFT OUTER JOIN [aspnetdb-somedbname].[dbo].aspnet_Membership ON [aspnetdb-somedbname].[dbo].aspnet_Membership.ApplicationId = [aspnetdb-somedbname].[dbo].aspnet_Users.ApplicationId 
AND aspnet_Users.UserId = [aspnetdb-somedbname].[dbo].aspnet_Membership.UserId;

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
);

INSERT INTO AspNetRoles(Id,Name)
SELECT RoleId,RoleName
FROM [aspnetdb-somedbname].[dbo].aspnet_Roles;

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

INSERT INTO AspNetUserRoles(UserId,RoleId)
SELECT UserId,RoleId
FROM [aspnetdb-somedbname].[dbo].aspnet_UsersInRoles;

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    [User_Id]    NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[AspNetUserClaims]([User_Id] ASC);

CREATE TABLE [dbo].[AspNetUserLogins] (
    [UserId]        NVARCHAR (128) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);



INSERT [dbo].[__MigrationHistory]([MigrationId], [Model], [ProductVersion])
VALUES (N'201312300649350_MergeMacheteContextAndIdentityDbContext', 0x1F8B0800000000000400ED5D59731CB9917EDF88FD0F0C3ED91B61F1D04C843D41D9C1A1285BB648D1A466F4C8285681ECB2EAE8A94324F7AFEDC3FEA4FD0B0BD4892381C255D54DBA5F186C00F521012412894402F97FFFF3BF277F794A93BDEFA828E33C7BB77FF4E6707F0F65611EC5D9C3BBFDBABAFFC31FF7FFF2E7FFFC8F93F3287DDAFBB52FF79694C35F66E5BBFD5555AD7F3A3828C3154A83F24D1A87455EE6F7D59B304F0F82283F383E3CFCD3C1D1D101C210FB186B6FEFE4BACEAA3845CD0FFCF32CCF42B4AEEA20B9C82394945D3ACEB96950F72E831495EB2044EFF62F029C54A137EF832AD8DF3B4DE200D3708392FBFDBD20CBF22AA830853FFD52A29BAAC8B3879B354E08922FCF6B84CBDD0749893ACA7F1A8BEB36E2F09834E260FCD0AA13F687E6E1069EE38EA89E09794D23DFED9F8655FC1D27D1A570B97FA0672601275D15F91A15D5F335BAEFBEFDF87E7FEF80FDEE80FF70F88CFA86548FFFCBAAB7C7FB7B9775920477091A7A8BEAD69B2A2FD05F51868AA042D1555055A8C0BDF731424D3384DAB9BA32FC77AA36354285BF75438830E5375550543D0C6624F425268459209D6791334E85084B173D0EE65B3CF9F6F72E82A74F287BA85684E79FF6F73EC44F28EA533AE05FB218CF558251D4E615E36145A5A2DA1F0E0F0FB5EA9DEEA7B0408463BCF479BD8EBC609DB534DD3D2BBAE0AD8F0EF8A52578868A2E83EFF1433335B92A6FE2872CCEF0E05EA3A4C92F57F1BA95936F7A09733B14FA50E4E9759E50D2A7CFBBBDC9EB2224532E9714F812140FA862293B3918C59A96B06BC15EA3C80BBA164ED7A9C6C1FF977926A04CCC96C7300C8A28AB53B7CA5394DEA1024BCDAA2ECD08C07D769F63A5A1195E1F937F2748961524833AE22C497A41219524BDA8D125AD9D124AC25AE8DBBEA4481D53402AECD8524E12EFAA25E5B54ABA4139FB39C73D1864C673E93E2ECA8AE889470A1EFF518FC76DAB3E76AF5A5D53126CAA8D7DCDB3373188A20295A587166AD5337B7BC2660648EB38FED1431D259E88485589873AFE3B5E93C28A5A8E7C74D77A9567CA4A7CB4E50165D1B86FB2536D5A8CBC526FC08E77FBA097A9BE9C7FC78B17BC0D6AB26EAF04D5804E17941626D35461F99A17DF089B01C474887D89911A26435050D85C27C5A469D96BD44BAEE09D93E12441A47BBE78B13E91C1F52200BEE422CCCEE2F36F2AE9FE9EC75933873FA6C10382451E5BE6B69DF294B081F20511081632158557F2BD9B965CE6052128B4ADE420DBBCD72810DBD639CAC3A6775C41767262431A913FE9C04F44A508D1A5B19D7A1A347605A534B67F27686CFF3A098D572B2B9A86B547AD9D5D27CE82E259EF744A03FA024F4F5AAB9A6BA2DDC709A24F1FC12DBC8F3DFC97559DDE65419CF8EFB0017AA94E5B0705E6928AB0D6DC551528CC8BE81B52DA5876AAE9F62E39DAA2B2DBE26E8DAC9CF637F87C4F68F6A1EB7CBEBF684ED2C882E685FB3EDFFF1C17D5CA194B71C06785748D9AD3004CCE7BCAA4A9BB4BF57392B0CA5394A0B21470D49F5D07A1B35E5B608C454C8A2B143FAC2A452DF39C223C4E55AB694336B5C1640F495CAE12F41D25CE438442BCB6054581B9347165353215E3EC979B532F933ACE4ABC165409306F4CD1E292948E9CDB4770E2040BF20895E1DC472D6950C415FECE83344AB01C798CAB55B88A93082B33AE1D91D5697ECF83D9911667211650AE6C4C1A1824D4598B6DCB501AE659855978424BF632C2436545B7C39B5BE9192A9C3C97FAC1878EE9C9ED262362F62E2F56791EB9324A9CA6F14311645581EEEB07E4CC2F615E63ACE7FC3E2FE287D8DDBCDE0C508BB51C07B6F52DCB846D9D8BF061AB8BA1A7755C340D8C405DCC742DC02B26D6609318AF9FA5331B753053344EF0225E30B2B2C6DC1D8A044D49E1EEBB9102D3DA614BE8C449FFB738498ECC3F3936FFE4ADB1C3DE6E7BBCAC45567116A2792CCC1FD1C087C6BAF43CE2CF4E4BE2B3994A0FCF3B68A1E848215C42B0C54A8A995A8C1F1B9852E1F54C55441504E91DF255D48E859C2CC75F99866F8F59C49F09B9EDACB68D48D463A63FFE5C44A87055305A2A5A474E431AFC9823D625AAA3DCD4811B6FB78A783DA19D1CF939B5EFB6D79F7C6CAF9BA5C719044FBE15995CC3F291D78D2DDAD8125417C9F3D7C00B90E38E94405C07D9488AE6B2FCEC58EF6F7540F6EFAD2DD30DEA3E4F92FCF17D5CA0B091B16E68589CC4551DF1FD616CCE49E2CE44E168F0C5DA60B9CE8B0AA7E3FD92E30E0E6FF1BB85513A7975EF76EDF4B66DD3DBD885CDAF9AC46B7413DA9419C93764071AC31A27AB8FDC721FB074CBCA816A93B4B08DAAD7A80452FA9BDCA94E070B81FD0E9784BADEF010ECF5DE7A6307DC50E9497252DBF32DB9215E56410A1C95E90194E8B71A513601ADDADBA1D95DB4DBAD269B584DFC6D5D152B88B8BFB51662AD207E8512EC3C5D27F9B3FBBE33CF923843C382E8B6850CF0AFA6C72F054133B1FB0DAEBAEDE719B1DBBB5E9A21766CBC2BBEF462A837DD147A38162493E026AED0A9B74B6A6E046CFAF6DA7C23A5E20FBD2B60A673C4CFC533C35ABDDDAADB987B5267FCB9C62A13DED83B7B0C707097B992138EBCD8AF923A0B5737F57A9DC4EE0DC0FFA74186D78AAB240851639B75441C2C0B17A85AB91FE80E701FC6C35C4BB3160D75FE847F39E2E9592D3DDE3522CA64CE58B6EC1FE3C11F7D48D0534C39BDBAB805EDD4E885AF37A4419C482E7C92ACDB416B6554683E4FBCF8C9173035BDF40AA584B436534A9D900D10289699EBE4CFC1C0029986D4A618B30D14597150A93CB5EC8ADC5286247E2F2514916CA7C4724E3BAA86C95EE36E0A9186D1F75EE7F3EBC1158DF76297D375EBBB7FA150ED123B4BC57779A492AE7FD45D646D548634AE4E3123A4EBCA713FD6EEE99C7D2183B2A7C7D8E3BCC2FBD915ADE5CD76523356758677D1DCA5F2D96ABDCE1FBBC73B07A5A6BB9744DEDEAC8B0265E1F34503D7D4C532114C8085BC38CBD375DDA8303BCD69DB34275AF570D59E84EBD932F5CA7299EC54A957B852FA7136F169752C505517D9595D56793A1A0EEDD142841BD8F2B77837C774F5AB4BDC4EE08E8FA9A7F5262C99FE9EC8B2AB7867757CE156C71025C9220F6D6DCABCD968F373736981EE11D67FA8B556EB4865FCECF3C44D3F2F54DE25F9C33A28AA388CD714835B0BBCDD4340FFB67A9CB1A949D4E6E4E628077798CE98F21AB5BAA1A35CF7B812EF949D617AEBA725639E54BA0BD24645E60B9595922A28B9B5202BEDDFFFCED2C43A4D366B8AB512359FF2FC5BBD7E8D3226C45F3DE4858717544C6D81E8A9BA3DBF5C5CD36FEBBD597E8781121456EE47CC657DE76FCC264CA3E44E87A1C790785B62C27731CEFE8667B0512DF7A4617FCFEF84AE9CE8B9BCC073457CFBD7DA5962A2BA350AE380BE5B607DDDB52ACED41BA9B7DE0E4110D6D2D45B61EDC3017575EA579BBCACD73B8565C3AF36F58B105999AD164FD1E43FBD78468AC61E1D4B4E25DC64FB844BA397404CFAD171882B53D82807BF9444D8FD1C94A8A38DE85AFDA03499CA66514863AFFEB58E23E30EBAC8EFE2043561D7663FB4FA589E6679F69CE6B5B381F713391BECA279C04F5259F5C3D54795DAE3A717CE278C607E6AF9943FA202458B5646F8D6D38453D777159425DE3F44FF243B09B5579ADF0A4FB3F2D14B64B7C989B25E17F977779DF463F9290FBFA1E873EDECEFD8AE8D5E261B99BD9FF28738F386D60FD0D98ADCC58D3C52197ECBEBCA0BDE073C15C93EB063A5D6B3A1F1DF77B3FE80B85FE32CCA1FFD0446E42A68E6C06CE4D3E83E1B71D65EDDF53C778D152E2223CF92204E97D7BAE63759340D5BC4FFA5A9E9D720A967A84A6A296B9536C048260CEE6D5B74B491C125041399A498DB5BD672ADF255A9FA8B6B1F7F0BCAD5EC95DDA0B02E48C83AFAE2EC02BCDE701F7CE264CDEDBCCFEDC4A4D025B5D124A6496D8A29491D4B2849A58A99924AC0A62925A594840E0594748EA59C9C9885A6DB0811F2B12848C0A24D1D38F97BDC180435BEE80BFF03892BD4A4A8EA295B5A5C71CD5CBA7AA6CF66AFDC6D49359AB9AA251598E0CE53C2D682663023481536ABF0A658BBA777736CD50C8A96986D4B2AC46CF347C5546329D393572DE637580C26A934E6FCD3B2CCC3B821AFA3190C2AC3B6F73C8BF67422CC8C864F2E46D3DE459D5431B1706292DEEDFF97D0A313350C8BE2584317FC86053EDAE7E7DEE7EC3D4A5085F64E9BA7B0B01A14946110899C87FB2B6253AE89FF18E9EF2039C3835915419C55E2DC8E33E2F0956890CF7DAB79864D681B6AE173DEA335098F99551AE3A353FD10794AA461A88AEBB7A96E3A39A078CE8815A1983D5246E10209CDC08A6C1422DACA0F016F1D2B32E42FCF8ACCF86855DFBFDABB11566462DCC918040E783721A3541C0747361D013B7AB68ED520BA176031A8FF75AABD1A028E6E84B7942FB8C95843EF39B79155B8676B0D9850EB2D38AEA6E1A934AE9EC3376F8E84AAAC584C87AA05584E671874C8E05FBBDD8C90E36F8C49E592F4FA1825ECDA8349036127BDD1CFF256E79EA804366AB4E85E2D2751E16B4D37BD7F396046C1ACA6CAA1EFAC85BEBC739690FDF24ED05274A9D7C336B608C0AF34A8E4F2C4930D1A833F134F6A3CD869BF36592F18CAEE5A68B550F68AEE52315C4DD818AFCABDDE552CA1E102CF2B12FD2513438D65DA7D5EA869FBF468AD862CC4B79343A743C7781B664B18177A0E53C548DCC5875998957BBF666921EE895399566C844799B1D2E5CE6D90AC462A80FEFA2FF20DB81D9B84DFE40ABEF1E5DB7DED6643AB6C9ACDC6F78935B80088B3E38FC5809790F58C0AFED94B68E7B2CC25F4C40B585AAF98C057B2E1664A414CB4804553468DB65EE8CF880576C702BC0636DCBEDE05D8ABBF0AD04A80DBB5DA100F9686D88D2D68A2B6C15568DBE8FDF1909290057849D9D73AF5AF376B7DEFE9BFBD995807F9822A7E5A4880092401FC67CAE24E4CC8F7CD82FCC7F7814ED541F7EDC6984FE204251BEF298F28F8A659EBD36720DBA65C262517DAB66E53AA6E87166F42FE5BFABCA91E2F1D02A44E644B73E7E84BA5C335806315CC34AD77961D6B8A7E5940255005DBC599423316664C61AC74EA873D1637C597DA421370A59B892F5FBEC4149AB109BE7CB1F292BA37A1C333D095219869DABB6596AC09DDD5589A37A51D7ADE544E1E59C643808AFEDE7610AE702DCDDBCB4FA2272AF9E40655AC321F933B18E783C3ABA09C0B7D05A23CF73B023954AF544F00B67B4008A8DF1D4E00349E4DD0F79D4BD9C4E7ACEF1D84C37B454E004A71F43E6F0D0FD0F7BDC545038089640002D116692D8AE403CEDA1F35C0FA87FA40A4EE10676AD0BBC818E2A0B7AE35939FB72E0D3042EFA1A2D52F638C0649C70CE75E1370ED635E104EF7CCD7144077BF4AE43B46DD9BE25E5E22AA1029996B003B856804D6DF7F532276BB3C03D8A9BE1CF5200E9492DF3219D3DD1BD8A38A829206BA5FC0AC5ECA1B0643EB00112768009A770A28CC5EEAF2EB19DB7EF3BEE984E464DF000EEFAA76B02EEF7EFA867572A73061288BBE613CAB813E917B5E337483BED793632987009A3CACE2CE6D567AFC027DA0EF21CC3448CB47986AA0B0AA2A3A4BCB2B98C3A6165877B611A21B00ACA3F46765C75EE6D14AB350B71EAB5848E6C3CAF5444FB3876E001E07867A62C2C9956B85DCCD95E98F41C1507689DC3B75AE5E913B534A269786E7A5C0FD6ADF4BADA669624ABA4B31552DFB4CF1942DDC6F9A5E80423BA7FD0005D131EAA0137D38EDF92760CFD37F9D96AFD373801BDA44BB5847345FBDC5BA9ECD393DF5E7A6F1C4D49F953A5372B3F3918F21AEEC22892F8FAC49A2378F7DF788CE3B33AEFDACAF09D0270A6714A601B03BCAB4CAA7009955C6C05E10400768B84B8007C9B0C304D516C134A6E818B58BC42C8AB57098AFE81BF8C01F6C8170E42FF6483CB1AF921ED5EB77AE457FC81ED9107B45E724DAE42C9ADE2A02A60C454F4D9C3E4B807DF7D678DE39D15992835183A351498B3AF38C664F8987A1142C08E5A38374B8497242677046E7AF8336C149F40B5AEA9E929D19999C1A491AD59B39353B0B3827F2D05BFD2324C311CF9077727013AE501A7409270724C8255A57759090189749D9675C04EB759C3D94E3975DCADECD3A08712BCEFE70B3BFF7942659F96E7F5555EB9F0E0ECA06BA7C93C6619197F97DF526CCD38320CA0F8E0F0FFF7470747490B6180721B318F00752434D555E040F88CB250F5046E8435C94E431D1E02E20CFD99C45A9504CEF40ABAF4C38D71207B037DFF69F90FF99B3B33784A0F1F84B3C47EDBEFC809B45D4D7A685085898C44FF1C737619004051047E32C4FEA34933B79C9BFCE9A57FEE8EFDB147D84AA79A392466853F411C8F3EDDDC3A4340C956C8685279A88D4241AB40A91012DB886F58906FDDB0613633AB84D326BD3F0C23FDFAE21C30C6F78E59FC71B32F4F1A897FE69342A591F8B7ACC9FC6A29245AC93036E56091E09C2CC158ECD59596022299EE5CAADA1B8E8CE3CED85860C601ED141BB74D2282A574F39DAE89B4C63C93D96E548D16318064594D529C7DD54BA3E5A8AD23BBCA36EA270B3786C8ED9F4BBCFBB5DBC3801A9AC9D8878E122A2DF6E5B8B06F8304E4324C83E9C511420400C18E901F744A323EAC7118B44A75BA01D4BD084D8462AB4240049A392CDB18E612C23BAC640C94CDF0FA9C648C72092114D6D6C631AA54DD147E80215D3105D923EC61098974619120D16A6368A30B32AB549FA180FC41F925326FB345394BC12F4522663B768BCF045A3F39FB05E33403F1F8D2543F2DD3C2BC615A8EEC99F8793232142F61761134A259B713079195664DF36D50CE94B2EE290B4DDEE7137CB054F31EBE9CEF9139BCFFB29807904C0F0DE2D0D217D04574145FF5829438AEC055339CE6E9A6CE534719D1DB69362D1B9D05446C805B8B84D36C4BA8853242E895C96C9562A416467C26FA4FA547DA42FAB3ABDCB1A9F7A1A8A4AB6C0825B0B641B28FD01B9A854054D6C5F46F5A733F4F10A14E645D44483A5D1A8E49DA47AE192AAF73DB11655B00F8D86AC927D388FB022A73B63A868FEDC67CC3163C0CFF7178D3995446E10B990CD3545FE392EAA1504DA656CD218DC7E778D1A6BD910135244E64B2C6D9B5BE5294A50C9B57A4CD547BA0E42415FEBD30C042AFE02B08450C9066D43F1C38A3B7CECD3F4511E0194476314943D2471B96A82D2F3BDC4E7192D3F587004458139281196203ACB6C6EC5D92F37A7E2CCEA924DB14ABC4054FC82CB651960C62559A5F9356D4835448A498CFB0895218047E519CCFCA088AB20292161C266195896F1AC7E8CAB55B88A93082B2A9C8159C835B044D4697E0FC37259FA98711662F9C173F9986AD6EE2011ECB454B2C10C4CC33CAB30038B1A2F9765815974018A24B863B6053660A8E6F33675889A1139789717AB3C8FF8F1E6F30CF8274DE38722C8AA02DDD70F886BBB986B70824142F116CFF97D5E907B563CC940B6E978B55FCA588CCEB54356311A5FC2AE0629BB31D9A69A107A5AC745435724D184F812063C8DD738AC4326315EF14A7E99E1F24C644FF3898A704911037EC40B4256D6989B430E9ACD3191BDDD472351BC18060A2C6F962BBFC549C29D6EF6698628C7008AD1B966F3C55B00E5ED6EFBFE2AB6EFEA8B4F06DB78EA390CBBEDBC0A609E6DBDF85E32B391997C4D598D3CBCF5CD834A1F019FA2740CE023D2290FEE2347F5B34D5E97A88E72C18B6D483598D6783B51C46B51303319C61BCB4F8A8DE527F38D6523FE78B821D100A72EC215717FE790C6641353455D24CF5F031E8C4E374313AC1E35745D4F8D711D6410415DB289B87F2E7939FF6C44CD6F7540F6ABADA58E85E2B20C4E08F224C91FDFC7056A5EA8E22814730DE665850563CDFBF18CA926469024EEF6EABC0984CA30308462F5A85CE74585D3F14683DB2889B926DB8FB4BBDBCAEE3A52C963112A6ED9291B5BAA6C8CD7741D4F0CACDDC6D59FCFA366B0CA0D8F25E61AECC3F2AA42C5F36D15A7A8AC82943B4600B2CDB14B72815FD88389B966FD216A46F2080372A49DDBFB4EBC6D9178933E7D6320DBC0B73A34059BE4DB79A41A1DD390F16752C43A94A3E5591267A8BF664AE3B13926EE0CF857D32397BC78E0B20CB661C155B7AD3923E650FE240EC8365180B20A6FBE2E05F3289361E64FCECB31F3D315B2B9BC892B740AFADC8BB9D6C8C74AE417E88BEFC387DE973FBF7FFF856EFB4C5EF7C15B1C6EE51032AD712F737E1CC002065A4D9D85AB9B7ABD4E629E6A2ECB60A45191061916CA574910A2F6415866D8817C8B2DD705AA56E25112906D81FD813F496273EC10CF9FF02F396C97BD39F350E39B8E55E31CB00AF079063D80BFFA90A0A758F0A36373761ADF0BD7F8BA071BADB5BDF69D65734D4FF2DD3C5A1E229589D743A864432CFE82C890686238BDFB170A39213B24EAE3DCE511C7666D8AA1B44BE3EA146F86D3356FBC12734DB537C1CA3CA41AAC7841D953C0AD77748691711273B1B8CAD1E93668CD8B25D0AD26491103EFBBFCF157F2821CBF6CD0E93B69FCE2A571FF5CAC8340EE9EADB791C9B24FE711CB7E4EEEFC6FBA0B54D545765697559E0A7EAA5C9E992B276E5ACB76258FCAE61988FFBAC46DE4DD7BC75403572F61CF9E196ED67757DAE7DC46872849001C2A79F92D394A851B41080ABCA1C2284870A14294CF74BA0DDA67C0C99CCF34986749FEB00E8A2A26319A787E10320D66DDEEA2F06B5DCCB957A61D0F0BFBF031B6A785D2EFE73B2E047D87BEDAF90E793C6EDBCD966D9C2D7DD823EB69D24547329F1FB20FE7991821EEF987BCE0959221D560DF8E9EAADBF34B6EBBDE279AE2DC403837468A124A5028CC8231D5C8360277139361603D20FE719CDDA04D323935E3BDC01E0D1DD2D238FB1BDE8B7087F27DA281CB56FC84A2BFE7779CABD6906AD0CF7981E782F0C01195BCC9D399728DC23810BDBEE874031EA88A3341E51D124DED7F086F95053590CBD2C714AE97EF2E964BB15ED092D63EBE6DBDA031F1FACC9735F5E7D2C58D1B712850ACFCEB4BC1867029B1216CEA5512E6596EEBA13925E7AB6173B7067A8ADC6074E0CFE7191D8A6A1E88CBD2C7BCC8EFE2049D2671C0ED62990C7DBC8FE5699667CF69CE7B7D3019FA789FC8F940F7AEAD78455FCC356DF9D5C74BA8DD4DB2891B9260470103982A5B9A3FA2024500149B638C4858549CD842A63EEE5550968F58C1F827D9900A071A62AE39F269563EF25A0D9F67C493EB75917FE757433ADD04ED531E7E43D1E7BAE2E1A80CD3C555E46D3ADD6CBE341145E0C942659961F6BD7FB622F72622181D2C644A7BF82DAF2B19F554A63EEE073C6F5034F04F7BDAD838C7B155A8CA39D6F635CEA2FC11786B7FBAB475CDCD3CD16E2D50DA4BCDBA2D977E633093DA7B1ADC34EA13419CB33C8BE226BCFDAF4152B30B691BCE84C67A1F13171FBCE50BB03630875634A1C37C2CBF347BB1DF817A26A1F7F72F449581D72497C5E86F41B982178C36471FF1068575413A47BCCBC0652DCE00D271DFBE611766161B1A7CD169E561B3D10517F232586DA875B7119360CC335B9BCA449F182AD910AB610900AC4BDF4A0EE8C2BA79E1803690BD1B074830541297E7823ECD644B812BC569DF63C1EAC8651948F0EE9B7FF0563426632B79C29385A88772E408334B911F862075F2287DDAF243C68665033C659918E22A9758A620E8FC2ABDEB44E2CD41F60120D4B8D8415A27562D1C648D271D34546E41571709CF92AE31BCB7196D7C3C3DE3B195C522D45D8CBBE2C60B2ED0A5EA1083BA1D0BCCCB5B684A198EB73AB0A2257166744D8FB51042912F3248912E65F83D8450ECC2173271159BE69028894D33CA2E94221FCFB02DB2BFD7AF3D58E77F2EF1A6B4659C9BDF92B3246EB6937D818B208BEF51597DC9BFA1ECDDFEF1E1D1F1FE5E63366DA35F9A476A44517A509611E35D4FAD29BD31188C577882D74A7E28FA61B846F7D4BC3DE04685FFF00498EBA47AF24055D54D95BF223C3AE4D8E62A2017B1B391C1F6F72EEB24210F3EBEDBBF0F12F1612F1E3E6BB67D540586DFB7A10FEDBF27A74F9DC1A1052109E4AE8C155213F3D011678879D8E264DF83E64996DFA5C1D3EFCDBBB7F54963A1F6F72E82A74F287BA856EFF67F383C3CA461AB42D4C4A1A60EC7931EBA6D389A74C4A20E26152D7E6BDC5EEA90D201975672B4A638F450C48B9DE8749042FBE9BA1EE2D580185ACC4B3DCF604F08FB28833D31EC430C1EE6D26E5ECE362FAF80107F2F7B3E0E4BE75D6C3E05E828818AEEFFF1D01EF9D800598761A8E8817E29A642097A2578BC843107AC6F6ADB2B1A0AC8E31F4D21BB2B1B2A4C53C8E1BE8402F4C8B8EDDD5D0E15A631A57DC442FB658A0954A8EAC49D0EB85D6B0D101BF0C52E35576AA54D0B838A2EE8B6D96BEF8D7B6058726D5C0EA3C352BBCDD90B9C98AAE07D2F76860E6100EDE7D6F0D4B8DBF4DC71EE6C9CFB9A18968AD4D7D6827BEE2ECE82E259B093E98C0417A8CFEB288F51FB948AB1B1664C85F0F3D10540F43EAFDDC0C4F1F38A4CC5F4DB69B82F471C4141F416934716A70CE35528B7158E0DADE781AF86B07A8E581A264D031C3E7E9E9BAAEC6EAD1A23E781183A44F441F3ECBB868A96E75554F5C1F3FC1AB51EA7518FCC51F9A07A0EBDC946D2B3E70DC2955D083D0FF3910A9EE78A36C4CE73681B172FCFABC18F8B9B673F9462A43CFB1673E1F1EC891A03E3B935AC8B8867DF222E0C9ED7210442E179D52AF888782A61F2C366CEF2F880780E1C2384C0B31F7420E69D83F94C8872370313F161EE66E02326D49D57568263DDB90A702EC89D3D374822DAB969556C2C3B6BBD088C5CE74699DA9AA47570D4C5AC730338760578EBE81CB0DB80CEBA0195857D7BB18631316E9C35FB31D1E2EC971E3E429C353DEE7BC0312C9CFD9CA45FFB566E8CCC4F6EF8A870F67D3E048473801823C1B520F7491ED86CBBC71870AE384E3B0C2AF69B83407E76A2818BF6660F24C677B3C71A23BBB9ECC4A9806E0E663621849B3DD618BC8D9DA556C6F1DD4ABC8029F855B9BB8A81D5ACA50E1048CD4DB916C3A759D3363EE4682F5477FEB83BB1612D36809B782F5666D0C1CBEC6702FB7ABA83B2CCC629B3DF468871C95C56752A1C99DFD30E7723B61890CCF3798C1096CCAB116FD27BD7B253D523756CCE98869EB65AA0E64EC18B1E1C0B41CD1C0E13A01866EA9DACB182C1063373904140F0327B34205A9987BDCA87D1CC6FB7B505C292B9C0E95A286C7D4BE9C0648E7A081B93CCEDFC76A767CDE78B2EC6FC7AB13A16153BCCF309581749CCF3FAD90716D317D05AB86DACB109CB88BEF462E28C39D81C8708630E07EE74603147A7232AA89807239224949807643A96580B57E48FDFFB14C9143ACBD375DD48B99DB4DD2E690B45F37AB102D7FDD4C6D786960F01E682C486FDB2471A037E39F83BCDB019B6B8776A82BBDBBA4A177373502A929836F03CF764F53530CFE34FC71CB336540951C6BC9228C41A7398F0BB7B832F6F595744F37AB16BFB573F1E221387483BA574F3DC0BC5DA7AB16C3B86A2522DEE16C6DE3E86975F456408E9E559BF19027CD9AF444C5C2FEDCED43221B771BE1C4E9D141E475AFEC07D802F6B12C6C85E60076BB9908E11BDACC9D03882D022858ADE65CF3043E02E95D0B2B3BD0DF1BB3C1854A62E939ACBEBDDFA34EFDD7669D02CCD554A78CB5463958A946D3B3AFEA3F16840E7DA3A6659E37E121FFF7F51FD34068CF030D9D978111E00B968111E10B9305F2D629DC578331137237A1F9335C20C9489F4E5814826D297F5922786F67233E25371BD3CB4F11C325E5821B1C1BDFC01CE3235C6985E1E41FB805E7E586F88E865CD794C1C2F6B143A7C971BE77271BBDCC1C0305D3E68A4A273B9C1A92271596BAED3A1B6BC120D04D4F245BA3456965B038650591E662217D3C769DDB7D26A80F7FA97506D6631545081763C0C0D1D69C703DC109B60F9310642E0E88DB12CCE09507422AC0DF0C5953C6CCD3427F594795694B9567846675ABC3C1BD86EF60CB8000E69B3B1E1ECC959A6AF55E16EF47A5A11B0462CAC8A2233DDE363556E07112305BA38B2FEA4E29D74F8ECDB91B7D01BAFE759B44746B97F01B6A382441479D3A55CD4491593FD20AE138FB7109BE673F61E25A8427BA7CDE5402CF883320C22B1F524568BAC76EE994B9A0C3E8BA5E7BF846AAEC9B92A99B541729667655504B11800EAAA883372329A30ADE74A691E3090A60D787CCE7BB4266F226715DC509D1A87D732C56A0774AEA7A7FA80898E63C444D0AB8EE3308AA3070EDA2B6522FD215D9E8986172E36C2440DB1B75750408371F8BA6C7AD8FAA445D84753027A6216A833E6E1167DD97635BC9CBD112E612FB1DE0E2F39C4E23DDD71D498CBBCF4E0B119EC181EBE7973240C238B493DD5C1A3D259B3F086FC82F23C1CA27A9A445223FFCCC666C48A4EBC437A86D3D6CF7686B729CBC81756BF15C8A0B2E6D171C47B15F3B0933A6EA5A9E6BE114682E2287202421C45E900BE5286321BE2E5996A2A4EE8228CD57AD7D3715EE5C3D979E2B303D9272EC252868CED4D32417710E6E12353B934DE91DF984ED4F2F023B348CBD9683B64D336A8508BC9264BFD69F065DD18670D1EC29DB2DDFD42D184BA0D28DA0BF2D5E8D72C9231E4CCA8982FC55212FF6D4985A35BF396B01364C6DD4631B559765A54489973D436C828A3A54F4B38BD88FDFF16AF5CEC939F9B660DD43DA0B50C63181B9B5E245318999F36BCF4B466CB5B30C2CBF69897DD7870CB6DCCCECCB9009BB071CE6FD79E4E23264406175D9DC6E3B35EF839832A8EBCA4C6F5660F1C7A8A6F6F26D68FBE20387C0BC98F4D33D2D0DCED64A5A0FB6463CC24788CDD02B71EA8A372FA6A047362CE642CC25AA2B39B8CA02E779EE373E965111993414E55FA4C36E1E227A954EACDB5348F9146DC023E69E2B03685A0116D33166731293D63E6AC0C2676DAFC0CA65D27EC02B829FE7A91226CD3FCB50901A6CD5F5B23BF1AFFF097C960ADD3BF8CA02EF7D5B11870D541C163B70B31D979431F79E20E7F818AE18E49843EC44549AE09057741292E92E4AB1B54713B81FDBDF3C1B95650F36FC2154A8377FBD11D7978B175D0ED7263F2428EB00784EBE8B574694D7D01457DCFFD8E65AAD27ECF2A54D6674095B479D3E09D7F9C80DDA543D04DD63432EFAF2954C11780EA62CB4C572AAB4B518526726FE811A0FB0C08BBCDD303A74DD2602574015965B4895DAF45525666B3E5ADD36563EA5C08ACABCB9355D4795E4CF273EB0D26F2739B0EF233C9D241EEDD3800F03E0BC66F7375476438B0920CC9902F1F93AEC8748DDD2B3D62557D0654479BA73119993D8A3827996C505296EB4B54917CFDCA5A7D425A599B2DAF8CE49B55D6E90ACA1ABB32EA6A9B426675777B7965DD5D1975DD4D21B3BA2746752CA2AE596F74796738787E4F8811B68C5829A59EC896B1EE06CE1E55145CCDA09B3A8C2ECAACBBB8B62145D0B7E49723A88FF92C5E2B669B66DEEC0E76B2D9E06D06F17609453948F0C69ACDDC72009A2BBF05019E3C50A4F6498A866AF28445B3946EF94033F5DDF881F33156A368DAC16628BA005684060C3ACB7DAC25AEE7D0B0EB78A9B36349EB21ED58B629AAF10785DCF83195E5BFF19DBBB44EE321CF6A6114C53648C9DF602788AEBD60074C7800730D60F5C48EF43E7182F90DBACD520EC0AEA81221A0E1B7EA6DE4979CFB6ACF49496768BA5A0222111086935D216C04A88F879C193AA21B289D2E001DDF3C72C3925DA03B238CA783DDE82F3B11381F2C75D325BE5ADED8DE507FB06834EB6D04B456E18EE441CFD3EE1C8BA6C11E324013355C693C34153698365FF359DE9A3EB882285A0DBB8B80A483446F4DA3252E0B40D3759C1B144737F4968DC9507484D45421C074B95EBB633C5D9FE80DC931BCE2201E68459BA1D919529431D37F57E83086E4C47846BE58BA2BA833CB89BE909D6ECED8198C2D5180E972B5BBA37F666638591BF24E0E5AFB5897807F5679113CA08B3C4249D9A49E1C5CD719791BAEFDF51E35CEE23DC409C6CCDA50F223685FE663769FF7278A1C457D11FEC94D5405515005A74515DF072189E61E22ACE6640FFB7BCDFB6364077587A28FD9E7BA5AD7156E324AEF12460C93834955FD270702CD279F9BA087A58F26603263F29CDEE7ECE73A4EA281EE0FC0F33F120872E2D9BD0547C6B2226FC23D3C0F48974DC0161DA0AEFB8683DAFEA5EBF273761390D04A32DAA6FB90EDB193F771F0500469D9618CDFE39F98FDA2F4E9CFFF0F4858BB23A7E50100 , N'6.0.2-21211')

select * from dbo.__MigrationHistory