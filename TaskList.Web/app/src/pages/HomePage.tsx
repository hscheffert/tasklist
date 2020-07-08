import * as React from 'react';
import { Link } from 'react-router-dom';
import { Button } from 'antd';
import bindAllOfThis from 'utils/BindThisHelper';
import { PageProps } from 'models/frontend/common/ComponentProps';
import Routes from 'config/ConfigureRoutes';

// Dev note: Page props denotes a component as a page. It also include comments for `this.props.*` which are nice.
interface HomePageProps extends PageProps<{}> {
}

interface HomePageState {
}

class HomePage extends React.Component<HomePageProps, HomePageState> {
    constructor(props: HomePageProps) {
        super(props);
        bindAllOfThis(this, HomePage.prototype);

        // You are allowed to set defaults here, based on your HomePageState state object
        this.state = {
        };
    }

    render() {
        return <div>
            <h1>Welcome to the QCI React Seed Project!</h1>
            <p>This project is aimed at taking the step by step process that you would normally use to create a new project and simplifying it.</p>
            <p>The project itself contains some 'nice to haves' and some 'i didn't know i needed that'. Redux-Thunk and the Redux dev tools are notoriously finicky to get installed and working. Guess what? They are installed and working.</p>
            <p>There are some examples from other larger projects. The LayoutComponent, bindAllOfThis and Routes files are perfect examples. They are part of the 'core' project, edit with caution.</p>
            <br />
            <p>There is plenty more. Feel free to remove what you don't need, like extra pages, and be sure to commit you changes to your own git repo!</p>
            <br />
            <h2>Examples</h2>
            <p>Buttons must be nested under the {"<link>"} tag, not the other way around. FireFox will <i>refuse</i> to work with links in the {"<button>"}</p>
            <Link to={Routes.GET.FREQUENCY_BASE}>
                <Button type="primary">Go To Frequencies</Button>
            </Link>
        </div>;
    }
}

export default HomePage;
