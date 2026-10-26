import { createTheme } from '@mui/material/styles';
import type { Theme } from '@mui/material/styles';

export const createAppTheme = (mode: 'light' | 'dark'): Theme =>
  createTheme({
    palette: {
      mode,
      ...(mode === 'light'
        ? {
            background: {
              default: '#f8fafc',
              paper: '#ffffff',
            },
            primary: {
              main: '#2563eb',
            },
          }
        : {
            background: {
              default: '#0f172a',
              paper: '#1e293b',
            },
            primary: {
              main: '#3b82f6',
            },
          }),
    },
    typography: {
      fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    },
  });
