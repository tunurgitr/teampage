# TeamPage

The thought here is to create a system to connect Agencies and their Clients together. In my experience, there's often not great ways to do this, or the ways available solutions are expensive and require too much customization to get started.

This is a weekend-project to learn more about the changes in MVC core and EF core.


## Hosted on azure: https://team-page.azurewebsites.net

### Quick Demo run-through
Normally, an Agency and a Client wouldn't be created for the same login, but this is the easiest way to see the features:
1. Click "Register" in the top-right corner of the screen
2. Fill in the info and click "Register" (email doesn't need to be real, no emails will be sent)
3. Click the "Setup a new Agency" link towards the bottom of the page.
4. Put in some fake info for a company -- make sure to use url-friendly text in UniqueCode (I haven't gotten to the validation there yet).
5. Click Register
6. You're now at the Agency page
7. Click the "Client Registration Link" to register a new client company under this same login
8. Fill out that form and click register
9. Now you're taken to a page where you can select what company you want to work with
10. You're probably not blown-away by this -- the real work was in finding the best patterns for a solution like this on .Net Core.

## Notes

After an initial trial implementation, it's clear a few major changes are in-order:

This is admittedly not a super-complex MVC app, so I'd like to lay out where I'm at and what I'd see as the next steps:

#### Highlights

- I like what I've started with the `BLL` folders, specifically looking at `Clients` and `Agencies`. The `Service` classes hide the specific implementation from the controllers in a way that (I think) improves readability of both the controller and the service class.

	This seems like a good way to organize a basic MVC app.

- This is a "Starter UI". I have notes below on using something like AdminLTE.

- I held off on implementing projects as I finished the first section, there's some notes below on what I'm considering changing before moving on.

#### Next Steps

##### Consider Bounded Contexts
- I had also sketched out a more complete data model that included Projects, ProjectItems as an abstract class, and Attachments that could be linked to Project items. Those all had links back into clients and users.

- After getting this far, it's clear to me that we have a at-least 3 distinct [Bounded Contexts](https://martinfowler.com/bliki/BoundedContext.html) (BCs):

	- Authentication (the built-in ASP.Net elements)
    - Management (handles CRUD of companies and users and linking them together)
    - Projects (handles projects)

	I'd argue that it might be wise to consider separate EF Contexts for these areas if this weren't being designed to run at scale.  Even if scale isn't an issue, the BCs are so separate that the models start to mean totally different things. 

	For example, in the context of Authentication, a `User` means one thing -- but the `Projects` context, almost everything is different. Trying to keep "one model to rule them all" can get messy.

- These Bounded Contexts need to be linked together. If a user is removed from a company, that message needs to get back to the Authentication system.  There are plenty of ways to do that, like:

	- Using an in-app library like [MediatR](https://github.com/jbogard/MediatR) to handle events within the same transaction. Unfortunately, it's not out for core yet.
    - Using a service bus (like Azure) and separate projects that make the updates in the background
	- Keeping everything in a single context for now, but making it clear with tables what BC they're owned by

##### Authentication

- I'm using the boilerplate MVC authentication code (for the most part) as I ran into **[this issue with Authentication handlers and Kestrel](https://github.com/aspnet/Security/issues/967)**. I tried the initial fixes, and eventually decided I'd use MS' boilerplate code and revisit this issue later. This boilerplate code is actually surprisingly adequate for a simple scendario.

- Implementing relying-party-login. I've done that in another test project, and it's relatively trivial to get going, but I want time to thoroughly review MS' boilerplate code around linking accounts to ensure I fully understand all the ramifications.

	I'm roughly-familiar with the pipeline in Katana on ASP.Net 5, but it's changed significantly in Core.
  
##### Validation

- There's not much setup, even basics like reporting back on duplicate checks and ensuring that the "unique codes" are URL-friendly.
- Normally, I prefer [FluentValidation](https://github.com/JeremySkinner/FluentValidation) for MVC projects, because I can use the same exception structure in service classes. 

	For example, I can throw a `ValidationFailureException` in a service class and that failure bubbles out to the view. With attributes, it gets tricky to plug-in to the same pipeline. 

- I've also done some work to create Angular pipeline handlers that pickup FluentValidation's JSON if it's returned from an API call -- so another +1 for FluentValidation.
- However, I don't want to do too much validation before ensuring that the structure of the app makes sense.

##### UI / AdminLTE

- This is the default Bootstrap theme -- I had started working on integrating AdminLTE using [Bower](https://github.com/almasaeed2010/AdminLTE). There's also a [nuget package](https://github.com/eralston/AdminLteMvc) that is a helpful starting point for core.
- I'd plan to fork the AdminLTE nuget package and try to structure something that worked well in core and that took advantage of Tag Helpers to implement components. That in-and-of-itself is not a small task, but potentially a helpful one as AdminLTE has worked well for me in the past. Ideally I'd like that to work alongside the bower package.



##### Consider moving Agents, Clients to a "Company" concept

- Strictly separating Agencies and Clients is too rigid and awkward as it comes to the view.
- Real-life relationships are complex -- some companies may act in an "Agency" and "Client" role in different scenarios.
- The contextual relationship is most applicable in the context of a specific Project, not as an absolute.
- The UI is already hinting at this as it wraps clients and agencies into "Companies" to simplify the mental model for the user. I had started with fully-distinguishing them on the UI, but I quickly realized this was confusing.
- I need to talk this through and build out some mock-UIs before moving forward.




