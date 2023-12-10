import { ILinkData } from "../helpers/interfaces";
import { NavLink } from "../ui/NavLink";

interface IHeaderNavLinksProps {
    links: ILinkData[];
}

export function HeaderNavLinks({ links }: IHeaderNavLinksProps) {
    return (
        <nav className="header__nav">
            {
                links.map(link => <NavLink link={link.path}>{link.title}</NavLink>)
            }
        </nav>
    );
}