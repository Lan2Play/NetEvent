Permission system
==================================================
Inside of NetEvent the permission system works like that:

- to every user can be assigned to one role
- to every role can be multiple permissions assigned
- permissions are are basically role claims that are defined as policies in ``NetEvent/Shared/Policies.cs`` and that can be used 