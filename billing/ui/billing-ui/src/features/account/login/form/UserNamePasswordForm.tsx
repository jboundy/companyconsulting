import Grid from '@mui/material/Grid';
import { Button, TextField } from '@mui/material';
import { Controller, useForm, type SubmitHandler } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Google as GoogleIcon, Facebook as FacebookIcon } from '@mui/icons-material';

const loginSchema = z.object({
  username: z.string().min(1, 'Username is required'),
  password: z.string().min(8, 'Password must be at least 8 characters'),
});

type LoginRequest = z.infer<typeof loginSchema>;

export default function UserNamePasswordForm() {
  const {
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<LoginRequest>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      username: '',
      password: '',
    },
  });

  const onSubmit: SubmitHandler<LoginRequest> = (data) => {
    console.log(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Grid container spacing={2} sx={{ maxWidth: 400, mx: 'auto', mt: 4 }}>
        <Grid size={{ xs: 12 }}>
          <Controller
            name="username"
            control={control}
            render={({ field }) => <TextField {...field} fullWidth label="Username" variant="standard" error={!!errors.username} helperText={errors.username?.message} />}
          />
        </Grid>
        <Grid size={{ xs: 12 }}>
          <Controller
            name="password"
            control={control}
            render={({ field }) => <TextField {...field} fullWidth label="Password" variant="standard" type="password" error={!!errors.password} helperText={errors.password?.message} />}
          />
        </Grid>
        <Grid size={{ xs: 12 }}>
          <Button fullWidth variant="contained" type="submit" sx={{ mt: 2 }}>
            Login
          </Button>
        </Grid>
        <Grid size={{ xs: 6 }}>
          <Button fullWidth variant="outlined" startIcon={<GoogleIcon />} sx={{ py: 1, textTransform: 'none', borderColor: 'divider', color: 'text.primary' }}>
            Google
          </Button>
        </Grid>
        <Grid size={{ xs: 6 }}>
          <Button fullWidth variant="outlined" startIcon={<FacebookIcon />} sx={{ py: 1, textTransform: 'none', borderColor: 'divider', color: 'text.primary' }}>
            Facebook
          </Button>
        </Grid>
      </Grid>
    </form>
  );
}
