/* eslint-disable react-refresh/only-export-components */

import type { RouteObject } from 'react-router-dom';
import { lazy } from 'react';
const Login = lazy(() => import('../features/account/pages/Login'));

export const AccountRoutes: RouteObject[] = [
  {
    path: 'account',
    children: [
      {
        path: 'login',
        element: <Login />,
      },
    ],
  },
];
