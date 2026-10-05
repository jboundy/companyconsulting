import { Grid } from '@mui/material';
import UserNamePasswordForm from '../form/UserNamePasswordForm';

export default function Login() {
  return (
    <>
      <Grid container spacing={2}>
        <Grid>
          <UserNamePasswordForm />
        </Grid>
      </Grid>
    </>
  );
}
