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
        }
    },
    module: {
        base: (moduleId: number | string) => `module/${moduleId}/`,
        full: (courseId: number | string, moduleId: number | string) => `/course/${courseId}/module/${moduleId}`,
        view: {
            base: 'view',
            full: (courseId: string | number, moduleId: string | number) => `/course/${courseId}/module/${moduleId}/view`
        }
    },
    lesson: {
        base: (lessonId: number | string) => `lesson/${lessonId}/`,
        full: (courseId: number | string, moduleId: number | string, lessonId: number | string) => `/course/${courseId}/module/${moduleId}/lesson/${lessonId}`,
        view: {
            base: 'view',
            full: (courseId: number | string, moduleId: number | string, lessonId: number | string) => `/course/${courseId}/module/${moduleId}/lesson/${lessonId}/view`
        },
        edit: {
            base: 'edit',
            full: (courseId: number | string, moduleId: number | string, lessonId: number | string) => `/course/${courseId}/module/${moduleId}/lesson/${lessonId}/edit`
        }
    },
    privacy: {
        base: 'privacy',
        full: '/privacy'
    }
}