import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import FrequencyTable from './FrequencyTable';
import FrequencyEdit from './FrequencyEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

class Frequency extends React.Component<{}> {
    render() {
        return (
            <React.Fragment>
                <BreadcrumbsItem name="frequencies" to={Routes.GET.FREQUENCY_BASE}>Frequencies</BreadcrumbsItem>

                <Switch>
                    <Route exact path={Routes.GET.FREQUENCY_BASE} component={FrequencyTable} />
                    <Route path={Routes.GET.FREQUENCY_EDIT} component={FrequencyEdit} />

                    {/* This needs to be the last item */}
                    <Route component={PageNotFound} />
                </Switch>
            </React.Fragment>
        );
    }
}

export default Frequency;
