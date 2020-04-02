# Blazor Blog
A cheap and cheerful blogging system built on Blazor
***
Here is a list of goals for Blazor Blog that guide the development of this project:
* **Simple** - I don't want or need any complications.
* **Fast** - I don't want to wait around all day.
* **Cheap** - I don't want to spend a lot of money on hosting.

If a feature is going to violate the above goals, I probably don't need it.

There will be separate apps for the public blog app and the private authoring app.

Intended tech stack for the public app:
* Blazor - Client-side
* Azure Storage for static website hosting
* Azure Storage for posts, drafts, media
* CDN for SSL and custom domain

Intended tech stack for the authoring app:
* Xamarin Forms, with Android, iOS, and UWP targets
* Azure Storage for posts, drafts, media
