import { ILinkData } from "../models/interfaces";
import { NavButton } from "../ui/NavButton";
interface IHeaderButtonsProps {
    links: ILinkData[];
}

export function HeaderButtons({ links }: IHeaderButtonsProps) {

    return (
        <div className="header__buttons">
            {
                links.map(item => <NavButton link={item.path} key={links.indexOf(item)}>{item.title}</NavButton>)
            }
        </div>
    );
}