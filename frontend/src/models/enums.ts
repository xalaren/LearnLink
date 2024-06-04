export enum InputType {
    text = 'text',
    password = 'password',
    rich = 'rich',
}
export enum ProfileEditActions {
    main = 'main',
    password = 'password',
    delete = 'delete'
}

export enum ViewTypes {
    created = 'created',
    subscribed = 'subscribed',
    unavailable = 'unavailable',
}

export enum NotificationType {
    error = 'error',
    success = 'success',
    default = '',
}

export enum ContentTypes {
    none = '',
    text = 'text',
    code = 'code',
    file = 'file'
};