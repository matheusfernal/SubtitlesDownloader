# SubtitlesDownloader
This console application built with .Net will iterate over the files of a chosen directory and offer to download a subtitle for each file. If a subtitle matching the moviehash is found, this is the one that will be donloaded. Otherwise, the prompt will offer several options so the user can choose wich one to download. This is a work in progress.
## Instructions
* Go to https://www.opensubtitles.com/en/users/sign_in to log in or https://www.opensubtitles.com/en/users/sign_up to create an account.
* After logget in, go to https://www.opensubtitles.com/en/consumers to create a consumer (this will allow you to use the api).
* Copy the consumer Api Key to the `appsettings.json` file inside the `SubtitlesDownloader.App` directory.
* Also copy your username and password to the `appsettings.json` file.
* Navigate to the `SubtitlesDownloader.App` directory and run the app with `dotnet run`.
