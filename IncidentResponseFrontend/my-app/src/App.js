import './App.css';
import UserRouteGroup from './components/routes/UserRouteGroup';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <div className="App">
      <div className="wrapper">
        <UserRouteGroup />
      </div>
    </div>
  );
}

export default App;
