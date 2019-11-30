To add anything that is being stored as an entity to the database structure 
you have to open the package manager console in tools and run:

1) enable-migrations
2) add-migration MigrationName
3) update-database

For the migration name I would just say to use whatever entity or class name you’re adding in as the migration
