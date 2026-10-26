import { Paper, Typography, Box } from '@mui/material';
import UserNamePasswordForm from '../form/UserNamePasswordForm';

export default function Login() {
  //TODO: not centered correctly from the box
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 8 }}>
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 450, borderRadius: 3 }}>
        <Typography variant="h5" sx={{ mb: 3, textAlign: 'center' }}>
          Welcome Back
        </Typography>
        <UserNamePasswordForm />
      </Paper>
    </Box>
  );
}
