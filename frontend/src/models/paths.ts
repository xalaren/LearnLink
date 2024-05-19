import { ProfileEditActions, ViewTypes } from "./enums";

export const paths = {
    home: '/',
    public: (pageNumber: string | number = '1') => `/public/page/${pageNumber}`,
    login: '/login',
    register: '/register',
    profile: {
        base: '/profile',
        edit: (action: ProfileEditActions | string = ProfileEditActions.main) => `/profile/edit/${action}`,
        courses: (type: ViewTypes | string = ViewTypes.created, page: string | number = '1') => `/profile/${type}/page/${page}`
    },
    course: {
        base: (courseId: string | number = '0') => `/course/${courseId}/`,
        view: {
            base: 'view',
            full: (courseId: string | number) => `/course/${courseId}/view`
        },
        participants: {
            base: (page: string | number = '1') => `participants/page/${page}`,
            full: (courseId: string | number, page: string | number = '1') => `/course/${courseId}/participants/page/${page}`
        },
        roles: {
            base: 'roles',
            full: (courseId: string | number) => `/course/${courseId}/roles`
        },
        module: {
            base: '/module',
            view: (courseId: number, moduleId: number | string) => `/course/${courseId}/module/${moduleId}`
        }
    }
}