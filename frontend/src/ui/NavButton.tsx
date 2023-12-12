import { useHistoryNavigation } from "../hooks/historyNavigation";

interface INavButtonProps {
    link: string;
    children?: React.ReactNode;
    className?: string;
}

export function NavButton({ link, children, className }: INavButtonProps) {
    const { toNext } = useHistoryNavigation();
    const classes = className || 'white-tp-button';

    return (
        <button className={classes} onClick={() => toNext(link)}>{children}</button>
    )
}