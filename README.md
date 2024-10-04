# URL Shortener

This project is a full-stack URL shortener application with a .NET backend and a Vue.js frontend.

## Backend

The backend is built with .NET and provides RESTful API endpoints for URL shortening and user authentication.

### Technologies Used

- .NET 8
- Entity Framework Core
- SQL Server
- JWT Authentication

### Setup

1. Navigate to the backend directory:
   ```
   cd url-shortener-backend
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Update the database:
   ```
   dotnet ef database update
   ```

4. Run the application:
   ```
   dotnet run
   ```

The API will be available at `http://localhost:5203`.

## Frontend

The frontend is built with Vue 3, TypeScript, and Vite.

### Technologies Used

- Vue 3
- TypeScript
- Vite
- Tailwind CSS
- Axios

### Setup

1. Navigate to the frontend directory:
   ```
   cd url-shortener-frontend
   ```

2. Install dependencies:
   ```
   npm install
   ```

3. Create a `.env` file in the root directory and add:
   ```
   VITE_API_BASE_URL=http://localhost:5203/api
   ```

4. Run the development server:
   ```
   npm run dev
   ```

The application will be available at `http://localhost:5173`.

### Building for Production

To build the frontend for production:

The built files will be in the `dist` directory.

## Features

- User registration and authentication
- Create shortened URLs
- Custom short URL creation
- Time-to-live (TTL) setting for URLs
- View list of shortened URLs

## Project Structure

### Backend

- `Controllers/`: API controllers
- `Models/`: Data models
- `Services/`: Business logic
- `Data/`: Database context and migrations

### Frontend

- `src/components/`: Vue components
- `src/services/`: API services
- `src/App.vue`: Main application component
- `src/main.ts`: Application entry point

## API Endpoints

- `POST /api/auth/register`: Register a new user
- `POST /api/auth/login`: Login and receive JWT token
- `POST /api/urls`: Create a shortened URL
- `GET /api/urls`: Get list of shortened URLs

For more details, refer to the API documentation or the controller files.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.
