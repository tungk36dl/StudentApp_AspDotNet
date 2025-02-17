// import * as React from 'react';
// import { AppProvider } from '@toolpad/core/AppProvider';
// import { SignInPage } from '@toolpad/core/SignInPage';
// import { useTheme } from '@mui/material/styles';

// const providers = [{ id: 'credentials', name: 'Email and Password' }];

// const signIn = async (provider, formData) => {
//   try {
//     console.log("Call login ....");
//     const response = await fetch('https://localhost:7024/api/NoAuth/login', {
//       method: 'POST',
//       headers: {
//         'Content-Type': 'application/json',
//       },
//       body: JSON.stringify({
//         email: formData.get('email'),
//         password: formData.get('password'),
//         rememberMe: true,
//       }),
//     });

//     console.log("Call login success.");

//     if (!response.ok) {
//       throw new Error('Login failed');
//     }

//     const data = await response.json();
//     localStorage.setItem('jwtToken', data.jwtToken);
//     alert('Login successful! JWT token stored.');
//   } catch (error) {
//     alert(`Login error: ${error.message}`);
//   }
// };

// export default function LoginSignup() {
//   const theme = useTheme();
//   return (
//     <AppProvider theme={theme}>
//       <SignInPage
//         signIn={signIn}
//         providers={providers}
//         slotProps={{ emailField: { autoFocus: false } }}
//       />
//     </AppProvider>
//   );
// }





import * as React from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AppProvider } from '@toolpad/core/AppProvider';
import { SignInPage } from '@toolpad/core/SignInPage';
import { useTheme } from '@mui/material/styles';


const providers = [{ id: 'credentials', name: 'Email and Password' }];

const signIn = async (provider, formData, setRoles, navigate) => {
  try {
    console.log("Call login ....");
    const response = await fetch('https://localhost:7024/api/NoAuth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email: formData.get('email'),
        password: formData.get('password'),
        rememberMe: true,
      }),
    });

    console.log("Call login success.");

    if (!response.ok) {
      throw new Error('Login failed');
    }

    const data = await response.json();
    localStorage.setItem('jwtToken', data.jwtToken);
    alert('Login successful! JWT token stored.');

    // Gọi API lấy roles
    const rolesResponse = await fetch('https://localhost:7024/get-roles-by-user', {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${data.jwtToken}`,
        'Content-Type': 'application/json',
      },
    });

    if (!rolesResponse.ok) {
      throw new Error('Failed to fetch roles');
    }

    const roles = await rolesResponse.json();
    setRoles(roles);
    localStorage.setItem('userRoles', JSON.stringify(roles));

    // Chuyển hướng sang Home
    navigate('/');
  } catch (error) {
    alert(`Login error: ${error.message}`);
  }
};

export default function LoginSignup() {
  const theme = useTheme();
  const navigate = useNavigate();
  const [roles, setRoles] = useState([]);

  return (
    <AppProvider theme={theme}>
      <SignInPage
        signIn={(provider, formData) => signIn(provider, formData, setRoles, navigate)}
        providers={providers}
        slotProps={{ emailField: { autoFocus: false } }}
      />
    </AppProvider>
  );
}
