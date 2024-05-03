import { useHistoryNavigation } from "../../hooks/historyNavigation";

interface INavLinkProps {
    link: string;
    children?: React.ReactNode;
    className?: string;
}

export function NavLink({ link, children, className }: INavLinkProps) {
    const { toNext } = useHistoryNavigation();
    const classes = className || 'white-link';

    return (
        <a className={classes} onClick={() => toNext(link)}>{children}</a>
    )
}