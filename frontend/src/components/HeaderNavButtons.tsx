import { ILinkData } from "../helpers/interfaces";
import { NavButton } from "../ui/NavButton";
interface IHeaderNavButtonsProps {
    links: ILinkData[];
}

export function HeaderNavButtons({ links }: IHeaderNavProps) {

    return (
        <nav className="header__nav">
            {
                links.map(link => <NavButton link={link.path}>{link.title}</NavButton>)
            }
        </nav>
    );
}