export enum InputType {
    text = 'text',
    password = 'password',
    rich = 'rich',
}

export enum Paths {
    homePath = '/',
    loginPath = '/login',
    registerPath = '/register',
    profilePath = '/profile',
    userCoursesPath = '/profile/courses',
    editUserPath = '/profile/edit/user',
    editPasswordPath = '/profile/edit/password',
    courseViewPath = '/courses/id',
}

export enum EditActions {
    editUser = 'editUser',
    editPassword = 'editPassword',
}

export enum ViewTypes {
    created = 'created',
    subscribed = 'subscribed',
}

export enum NotificationType {
    error = 'error',
    success = 'success',
    default = '',
}