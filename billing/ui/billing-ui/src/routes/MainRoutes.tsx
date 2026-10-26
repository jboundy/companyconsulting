import { useRoutes } from 'react-router-dom';
import MainLayout from '../components/layout/MainLayout';
import { AccountRoutes } from './AccountRoutes';

export default function MainRoutes() {
  return useRoutes([
    {
      element: <MainLayout />,
      children: [...AccountRoutes],
    },
  ]);
}
