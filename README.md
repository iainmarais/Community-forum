# Community forum

## Description

Community forum is a forum platform developed using Vue.js, TypeScript, and C#. The application is in its pre-alpha stage and currently includes basic forum functionalities. 
Future expansions will incorporate an image gallery and a chat function. 
There are also plans to develop a mobile client using Dart and Flutter.

## Features

- **Forum**: Engage in discussions with users.
- **Image Gallery**: Showcase images (coming soon).
- **Chat Function**: Real-time communication (coming soon).

## Getting Started

### Prerequisites

- Node.js
- .NET Core SDK
- Vue CLI

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/iainmarais/Community-forum.git
   cd Community-forum
   ```
2. **Install the server dependencies:**
  ```
   cd server
   dotnet restore
  ``` 
4. **Install the client dependencies:**
  ```
   cd ..\ForumClient
   npm install
  ```

### Starting the client and server:

server: 
```
cd server
dotnet run
```
client:
```
cd ..\ForumClient
npm run serve
```

### Development
To start development, ensure both the server and client are running. Changes in the client will be automatically reflected in your browser, while the server may require a restart for changes to take effect.

### Contributing
Feel free to submit issues and pull requests. For major changes, please open an issue first to discuss what you would like to change.

### Future Plans

Image Gallery: An interface to display and interact with images.
Chat Function: A real-time chat feature for user communication.
Mobile Client: Development of a mobile client using Dart and Flutter.

### Licence
This project is licensed under the MIT License.
