export const MainRoutes = {
    MAIN: {
        path: 'main',
        title: 'Main',
    },
    HOME: {
        path: 'home',
        title: 'Home',
    },
    SEARCH: {
        path: 'search',
        title: 'Search',
    },
    PLACE_DETAILS: {
        path: 'place-details',
        title: 'Place Details',
    },
} as const;

export const AuthRoutes = {
    AUTH: {
        path: 'auth',
        title: 'auth',
    },
    LOGIN_WITH_GOOGLE: {
        path: 'login-with-google',
        title: 'Login with Google',
    }
} as const;
