import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import FrequencyTable from './FrequencyTable';
import FrequencyEdit from './FrequencyEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';

class Frequency extends React.Component<{}> {
    render() {
        return (
            <Switch>
                <Route exact path={Routes.GET.FREQUENCY_BASE} component={FrequencyTable} />
                <Route path={Routes.GET.FREQUENCY_EDIT} component={FrequencyEdit} />

                {/* This needs to be the last item */}
                <Route component={PageNotFound} />
            </Switch>
        );
    }
}

export default Frequency;
