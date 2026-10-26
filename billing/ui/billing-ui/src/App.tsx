import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { useMemo } from 'react';
import { ThemeProvider } from './theme/ThemeProvider';
import { useTheme } from './theme/ThemeContext';
import { createAppTheme } from './theme/defaultTheme';
import MainRoutes from './routes/MainRoutes';
import { BrowserRouter } from 'react-router-dom';
import './App.css';

function ThemedApp() {
  const { theme } = useTheme();
  const muiTheme = useMemo(() => createAppTheme(theme), [theme]);

  return (
    <MuiThemeProvider theme={muiTheme}>
      <CssBaseline />
      <BrowserRouter>
        <MainRoutes />
      </BrowserRouter>
    </MuiThemeProvider>
  );
}

function App() {
  return (
    <ThemeProvider>
      <ThemedApp />
    </ThemeProvider>
  );
}

export default App;
