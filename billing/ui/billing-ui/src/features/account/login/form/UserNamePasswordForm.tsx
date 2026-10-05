import Grid from '@mui/material/Grid';
import { Button, TextField } from '@mui/material';
import { Controller, useForm, type SubmitHandler } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

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

  const onSubmit: SubmitHandler<LoginRequest> = (data) => console.log(data);
  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Grid container spacing={2}>
          <Grid size={6}>
            <Controller
              name="username"
              control={control}
              render={({ field }) => <TextField {...field} label="Username" variant="standard" error={!!errors.username} helperText={errors.username?.message} />}
            />
          </Grid>
          <Grid size={6}>
            <Controller
              name="password"
              control={control}
              render={({ field }) => <TextField {...field} label="Password" variant="standard" type="password" error={!!errors.password} helperText={errors.password?.message} />}
            />
          </Grid>
          <Grid size={12}>
            <Button variant="contained" type="submit">
              Login
            </Button>
          </Grid>
        </Grid>
      </form>
    </>
  );
}
