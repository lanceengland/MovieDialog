# StarWarsDialog

This is a really quick ASP.NET Core API app that will return lines of dialog from the original Star Wars screenplay. I wanted a small app that I can practice deployment options: 

- Virtual Machines, Windows or Linux
- API app
- Simple container
- Kubernetes
- CI/CD with webhooks, etc
- Azure Functions

To Do:

- Add data folder to the deployment config
- Add the other two movie script data files
- Add file paths to config
- Add the other two movies to the API
- Add Swagger
- Upgrade to VS 2022
- Convert into generic movie dialog API (still only support 3 Start Wars movies)
- API would be movie/lines, movie/line/3, movie/lines/random
- Return JSON with speaker and line properties (instead of just line of dialog text). For example, instead of 

LUKE: Are you all right?  What's wrong?

return

{"speaker": "LUKE", "line": "Are you all right?  What's wrong?"}