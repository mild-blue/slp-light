This is stub repository for interviewing purposes within Mild Blue s.r.o.

It is related to slp.blue application, developed for quality management in medical laboratories.

### Goals

Test candidate's abilities to:
- get project up and running
- solve real scenario problem
- pose questions
- create PR

### Current Assignment

We are using third party application called Authentik to handle login and user management. From this service we are synchronizing users to the main application, every five minutes.

For some internal reasons, Authentik creates users prefixed with `ak-outpost`. Unfortunately, this got synced into the application and looks bad for users (e.g. if you are selecting someone to revise a document, you want just real people).

The task here is to:
- modify synchronization code to not sync such users into main application
- handle already synced-in users

There is one controller used to return all users from db. The application uses Swagger, for nice overview of endpoints (currently just one).
