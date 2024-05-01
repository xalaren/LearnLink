import { useHistoryNavigation } from "../hooks/historyNavigation";

interface INavButtonProps {
    link: string;
    children?: React.ReactNode;
    className?: string;
}

export function NavButton({ link, children, className }: INavButtonProps) {
    const { toNext } = useHistoryNavigation();
    const classes = className || 'button-white-tp';

    return (
        <button className={classes} onClick={() => toNext(link)}>{children}</button>
    )
}