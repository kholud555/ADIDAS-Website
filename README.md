# Delivery App - Deliveryman Registration

A modern Angular 20 application for registering delivery personnel with integrated GPS location selection and comprehensive form validation.

## Features

- ✅ **Modern Angular 20** with standalone components
- 🎨 **Beautiful UI** with food delivery themed colors
- 📍 **GPS Location Selection** using free Leaflet maps
- 🔐 **Advanced Form Validation**:
  - Password must contain uppercase letter and special character
  - Phone number must start with "01" and be 11 digits
  - Email validation
  - Username validation
- 🗺️ **Interactive Map** for location selection
- 📱 **Responsive Design** for mobile and desktop
- 🚀 **Real-time API Integration** with backend

## Color Scheme

The application uses a carefully selected color palette perfect for food delivery services:

```css
--first-color: hsl(19, 64%, 54%);      /* Primary orange */
--first-color-alt: hsl(32, 98%, 51%);  /* Accent orange */
--title-color: hsl(20, 16%, 15%);      /* Dark text */
--text-color: hsl(19, 16%, 35%);       /* Regular text */
--text-color-light: hsl(19, 8%, 55%);  /* Light text */
--body-color: hsl(18, 100%, 96%);      /* Background */
--container-color: hsl(26, 67%, 84%);  /* Container */
--second-color: hsl(134, 29%, 63%);    /* Success green */
```

## API Integration

The application connects to the backend API endpoint:

```
POST http://localhost:5000/api/DeliveryMan/apply
```

**Request Body:**
```json
{
  "userName": "string",
  "password": "string",
  "email": "user@example.com",
  "phoneNumber": "01894247823",
  "agreeTerms": true,
  "latitude": 0,
  "longitude": 0
}
```

## Form Validation Rules

### Password Requirements
- Minimum 8 characters
- At least one uppercase letter (A-Z)
- At least one special character (!@#$%^&*()_+-=[]{}|;':",./<>?)

### Phone Number Requirements
- Must start with "01"
- Must be exactly 11 digits
- Example: `01894247823`

### Email Requirements
- Valid email format
- Must contain @ symbol and valid domain

### Location Requirements
- User must click on the map to select their delivery location
- GPS coordinates are automatically captured and sent to the API

## Installation & Setup

1. **Clone or download the project**
2. **Install dependencies:**
   ```bash
   npm install --legacy-peer-deps
   ```

3. **Start the development server:**
   ```bash
   npm start
   ```

4. **Open your browser and navigate to:**
   ```
   http://localhost:4200
   ```

## Usage

1. **Fill out the registration form:**
   - Enter a username (minimum 3 characters)
   - Provide a valid email address
   - Enter phone number starting with "01" (11 digits total)
   - Create a secure password meeting all requirements
   - Agree to terms and conditions

2. **Select your location:**
   - The map will attempt to detect your current location automatically
   - Click anywhere on the map to set your delivery location
   - The selected coordinates will be displayed below the map

3. **Submit the form:**
   - All validations must pass
   - Location must be selected
   - Terms must be agreed to
   - Click "Register as Deliveryman" to submit

## Technologies Used

- **Angular 20** - Latest version with standalone components
- **TypeScript** - Type-safe development
- **Leaflet** - Free, open-source mapping library
- **OpenStreetMap** - Free map tiles
- **Reactive Forms** - Advanced form handling and validation
- **CSS Custom Properties** - Modern styling approach
- **Inter Font** - Clean, readable typography

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   └── deliveryman-registration.component.ts
│   ├── models/
│   │   └── deliveryman.model.ts
│   ├── services/
│   │   ├── deliveryman.service.ts
│   │   └── validators.service.ts
│   ├── app.component.ts
│   └── app.routes.ts
├── environments/
│   ├── environment.ts
│   └── environment.prod.ts
├── styles.css
├── main.ts
└── index.html
```

## Features in Detail

### 🗺️ Map Integration
- Uses free Leaflet library with OpenStreetMap tiles
- Automatic geolocation detection
- Click-to-select location functionality
- Real-time coordinate display

### 🔒 Security & Validation
- Client-side validation with immediate feedback
- Custom validators for all form fields
- Visual password strength indicators
- Form state management

### 🎨 Modern UI/UX
- Food delivery themed color scheme
- Smooth animations and transitions
- Loading states and feedback
- Mobile-responsive design
- Accessible form controls

### 📡 API Integration
- HTTP client for backend communication
- Error handling and user feedback
- Loading states during submission
- Success/error message display

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.

---

**Note:** Make sure your backend API is running on `http://localhost:5000` before testing the registration functionality.