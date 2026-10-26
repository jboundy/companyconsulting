import { useState } from 'react';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import { Outlet } from 'react-router-dom';
import Header from './Header';
import LeftNav from './LeftNav';

const drawerWidth = 240;

export default function MainLayout() {
  const [isDrawerOpen, setIsDrawerOpen] = useState(true);

  const handleDrawerToggle = () => {
    setIsDrawerOpen(!isDrawerOpen);
  };

  return (
    <Box sx={{ display: 'flex', width: '100%' }}>
      <Header onDrawerToggle={handleDrawerToggle} />
      <LeftNav open={isDrawerOpen} />
      <Box
        component="main"
        sx={(theme) => ({
          flexGrow: 1,
          p: 3,
          width: { sm: isDrawerOpen ? `calc(100% - ${drawerWidth}px)` : '100%' },
          transition: theme.transitions.create(['width', 'margin'], {
            easing: isDrawerOpen ? theme.transitions.easing.sharp : theme.transitions.easing.sharp,
            duration: isDrawerOpen ? theme.transitions.duration.enteringScreen : theme.transitions.duration.leavingScreen,
          }),
        })}
      >
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
}
