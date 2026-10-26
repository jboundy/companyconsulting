export const themes = {
  light: {
    background: '#FFFFFF',
    text: '#111827',
    primary: '#7C3AED',
  },
  dark: {
    background: '#0F172A',
    text: '#F9FAFB',
    primary: '#A78BFA',
  },
} as const;

export type ThemeMode = keyof typeof themes;
export type ThemeColors = (typeof themes)[ThemeMode];
