﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView
{
    public class GraphTraversal2
    {
        public static GraphViewConnection Connection { get; set; }

        public static void SetGraphViewConnection(GraphViewConnection connection)
        {
            Connection = connection;
        }

        internal List<GremlinTranslationOperator> GremlinTranslationOpList { get; set; }
        internal GremlinTranslationOperator LastGremlinTranslationOp { set; get; }

        public GraphTraversal2() {
            GremlinTranslationOpList = new List<GremlinTranslationOperator>();
        }

        public GraphTraversal2(GraphTraversal rhs)
        {
            GremlinTranslationOpList = new List<GremlinTranslationOperator>();
        }

        public GraphTraversal2(GraphViewConnection pConnection)
        {
            GremlinTranslationOpList = new List<GremlinTranslationOperator>();
        }

        internal List<RawRecord> next()
        {
            var sqlScript = LastGremlinTranslationOp.ToSqlScript();
            var str = sqlScript.ToString();

            var op = sqlScript.Batches[0].Compile(null, Connection);
            var results = new List<RawRecord>();
            RawRecord outputRec = null;
            while ((outputRec = op.Next()) != null)
            {
                results.Add(outputRec);
            }

            return results;
        }

        internal void AddGremlinOperator(GremlinTranslationOperator newGremlinTranslationOp)
        {
            GremlinTranslationOpList.Add(newGremlinTranslationOp);
            if (LastGremlinTranslationOp == null)
            {
                LastGremlinTranslationOp = newGremlinTranslationOp;
            }
            else
            {
                newGremlinTranslationOp.InputOperator = LastGremlinTranslationOp;
                LastGremlinTranslationOp = newGremlinTranslationOp;
            }
        }

        internal GremlinTranslationOperator GetStartOp()
        {
            return GremlinTranslationOpList.Count == 0 ? null : GremlinTranslationOpList.First();
        }

        internal GremlinTranslationOperator GetEndOp()
        {
            return LastGremlinTranslationOp;
        }

        //public GraphTraversal addE(Direction direction, string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)

        public GraphTraversal2 addE(string edgeLabel)
        {
            AddGremlinOperator(new GremlinAddEOp(edgeLabel));
            return this;
        }

        //public GraphTraversal2 addInE(string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)
        //public GraphTraversal2 addOutE(string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)

        public GraphTraversal2 addV()
        {
            AddGremlinOperator(new GremlinAddVOp());
            return this;
        }

        public GraphTraversal2 addV(params object[] propertyKeyValues)
        {
            AddGremlinOperator(new GremlinAddVOp(propertyKeyValues));
            return this;
        }

        public GraphTraversal2 addV(string vertexLabel)
        {
            AddGremlinOperator(new GremlinAddVOp(vertexLabel));
            return this;
        }

        public GraphTraversal2 aggregate(string sideEffectKey)
        {
            return this;
        }

        public GraphTraversal2 and(params GraphTraversal2[] andTraversals)
        {
            AddGremlinOperator(new GremlinAndOp(andTraversals));
            return this;
        }

        public GraphTraversal2 As(params string[] labels) {
            AddGremlinOperator(new GremlinAsOp(labels));
            return this;    
        }

        public GraphTraversal2 barrier()
        {
            return this;
        }

        public GraphTraversal2 both(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinBothOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 bothE(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinBothEOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 bothV()
        {
            AddGremlinOperator(new GremlinBothVOp());
            return this;
        }

        //public GraphTraversal2 branch(Function<Traversal<E>, M> function)
        //public GraphTraversal2 branch(Traversal<?, M> branchTraversal)

        public GraphTraversal2 by()
        {
            AddGremlinOperator(new GremlinByOp());
            return this;
        }

        //public GraphTraversal2 by(Comparator<E> comparator)
        //public GraphTraversal2 by(Function<U, Object> function, Comparator comparator)
        //public GraphTraversal2 by(Function<V, Object> function)

        public GraphTraversal2 by(GremlinKeyword.Order order)
        {
            AddGremlinOperator(new GremlinByOp(order));
            return this;
        }

        public GraphTraversal2 by(string key)
        {
            AddGremlinOperator(new GremlinByOp(key));
            return this;
        }

        //public GraphTraversal2 by(string key, Comparator<V> comparator)
        //public GraphTraversal2 by(T token)
        public GraphTraversal2 by(GraphTraversal2 traversal)
        {
            AddGremlinOperator(new GremlinByOp(traversal));
            return this;
        }
        //public GraphTraversal2 by(Traversal<?, ?> traversal, Comparator comparator)
        //public GraphTraversal2 cap(string sideEffectKey, params string[] sideEffectKeys)
        //public GraphTraversal2 choose(Function<E, M> choiceFunction)

        public GraphTraversal2 choose(Predicate choosePredicate, GraphTraversal2 trueChoice, GraphTraversal2 falseChoice)
        {
            AddGremlinOperator(new GremlinChooseOp(choosePredicate, trueChoice, falseChoice));
            return this;
        }

        public GraphTraversal2 choose(GraphTraversal2 traversalPredicate, GraphTraversal2 trueChoice, GraphTraversal2 falseChoice)
        {
            AddGremlinOperator(new GremlinChooseOp(traversalPredicate, trueChoice, falseChoice));
            return this;
        }

        public GraphTraversal2 choose(GraphTraversal2 choiceTraversal)
        {
            AddGremlinOperator(new GremlinChooseOp(choiceTraversal));
            return this;
        }

        public GraphTraversal2 coalesce(params GraphTraversal2[] coalesceTraversals)
        {
            AddGremlinOperator(new GremlinCoalesceOp(coalesceTraversals));
            return this;
        }

        public GraphTraversal2 coin(double probability)
        {
            AddGremlinOperator(new GremlinCoinOp(probability));
            return this;
        }

        public GraphTraversal2 constant(object value)
        {
            AddGremlinOperator(new GremlinConstantOp(value));
            return this;
        }
        public GraphTraversal2 count()
        {
            AddGremlinOperator(new GremlinCountOp());
            return this;
        }

        //public GraphTraversal2 count(Scope scope)
        //public GraphTraversal2 cyclicPath()
        //public GraphTraversal2 dedup(Scope scope, params string[] dedupLabels)

        public GraphTraversal2 dedup(params string[] dedupLabels)
        {
            AddGremlinOperator(new GremlinDedupOp(dedupLabels));
            return this;
        }

        public GraphTraversal2 drop()
        {
            AddGremlinOperator(new GremlinDropOp());
            return this;
        }

        public GraphTraversal2 E()
        {
            AddGremlinOperator(new GremlinEOp());
            return this;
        }

        public GraphTraversal2 emit()
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
                (GetEndOp() as GremlinRepeatOp).EmitContext = true;
            }
            return this;
        }

        public GraphTraversal2 emit(Predicate emitPredicate)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
                (GetEndOp() as GremlinRepeatOp).EmitPredicate = emitPredicate;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
                (GetEndOp() as GremlinRepeatOp).EmitPredicate = emitPredicate;
                (GetEndOp() as GremlinRepeatOp).EmitContext = true;
            }
            return this;
        }

        public GraphTraversal2 emit(GraphTraversal2 emitTraversal)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
                (GetEndOp() as GremlinRepeatOp).EmitTraversal = emitTraversal;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).IsEmit = true;
                (GetEndOp() as GremlinRepeatOp).EmitTraversal = emitTraversal;
                (GetEndOp() as GremlinRepeatOp).EmitContext = true;
            }
            return this;
        }

        //public GraphTraversal2 filter(Predicate<Traversal<E>> predicate)
        //public GraphTraversal2 filter(Traversal<?, ?> filterTraversal)
        //public GraphTraversal2 flatMap(Funtion<Traversal<E>, Iterator<E>> funtion)

        public GraphTraversal2 flatMap(GraphTraversal2 flatMapTraversal)
        {
            AddGremlinOperator(new GremlinFlatMapOp(flatMapTraversal));
            return this;
        }

        public GraphTraversal2 fold()
        {
            AddGremlinOperator(new GremlinFoldOp());
            return this;
        }

        //public GraphTraversal2 fold(E2 seed, BiFuntion<E2, E, E2> foldFunction)

        public GraphTraversal2 from(string fromGremlinTranslationOperatorLabel)
        {
            AddGremlinOperator(new GremlinFromOp(fromGremlinTranslationOperatorLabel));
            return this;
        }

        public GraphTraversal2 from(GraphTraversal2 fromVertexTraversal)
        {
            AddGremlinOperator(new GremlinFromOp(fromVertexTraversal));
            return this;
        }

        public GraphTraversal2 group()
        {
            AddGremlinOperator(new GremlinGroupOp());
            return this;
        }

        public GraphTraversal2 group(string sideEffectKey)
        {
            AddGremlinOperator(new GremlinGroupOp(sideEffectKey));
            return this;
        }

        //public GraphTraversal2 groupCount()
        //public GraphTraversal2 groupCount(string sideEffectKey)

        public GraphTraversal2 has(string propertyKey)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey));
            return this;
        }

        public GraphTraversal2 has(string propertyKey, Object value)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, value));
            return this;
        }

        public GraphTraversal2 has(string label, string propertyKey, Object value)
        {
            AddGremlinOperator(new GremlinHasOp(label, propertyKey, value));
            return this;
        }

        public GraphTraversal2 has(string propertyKey, Predicate predicate)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, predicate));
            return this;
        }

        public GraphTraversal2 has(string label, string propertyKey, Predicate predicate)
        {
            AddGremlinOperator(new GremlinHasOp(label, propertyKey, predicate));
            return this;
        }

        public GraphTraversal2 has(string propertyKey, GraphTraversal2 propertyTraversal)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, propertyTraversal));
            return this;
        }
        
        public GraphTraversal2 hasId(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.HasId, values));
            return this;
        }

        public GraphTraversal2 hasKey(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.HasKeys, values));
            return this;
        }

        public GraphTraversal2 hasLabel(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.HasLabel, values));
            return this;
        }

        public GraphTraversal2 hasValue(string value, params string[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.HasValue, values));
            return this;
        }

        public GraphTraversal2 hasNot(string propertyKey)
        {
            return this;
        }

        public GraphTraversal2 id()
        {
            AddGremlinOperator(new GremlinValuesOp("id"));
            return this;
        }

        public GraphTraversal2 In(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinInOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 inE(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinInEOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 inject(params object[] injections)
        {
            AddGremlinOperator(new GremlinInjectOp(injections));
            return this;
        }

        public GraphTraversal2 inV()
        {
            AddGremlinOperator(new GremlinInVOp());
            return this;
        }

        public GraphTraversal2 Is(object value)
        {
            AddGremlinOperator(new GremlinIsOp(value));
            return this;
        }

        public GraphTraversal2 Is(Predicate predicate)
        {
            AddGremlinOperator(new GremlinIsOp(predicate));
            return this;
        }

        //public GraphTraversal2 iterate()
        //{
        //    return this;
        //}

        public GraphTraversal2 key()
        {
            AddGremlinOperator(new GremlinKeyOp());
            return this;
        }

        public GraphTraversal2 label()
        {
            AddGremlinOperator(new GremlinValuesOp("label"));
            return this;
        }

        public GraphTraversal2 limit(long limit)
        {
            AddGremlinOperator(new GremlinLimitOp(limit));
            return this;
        }

        //public GraphTraversal2 limit(Scope scope, long limit)

        public GraphTraversal2 local(GraphTraversal2 localTraversal)
        {
            AddGremlinOperator(new GremlinLocalOp(localTraversal));
            return this;
        }

        //public GraphTraversal2 loops()
        //public GraphTraversal2 map(Function<Traversal<?, E2>> function)
        //public GraphTraversal2 map(Traversal<?, E2> mapTraversal)
        //public GraphTraversal2 mapKeys() //Deprecated
        //public GraphTraversal2 mapvalues() //Deprecated

        public GraphTraversal2 match(params GraphTraversal2[] matchTraversals)
        {
            AddGremlinOperator(new GremlinMatchOp(matchTraversals));
            return this;
        }

        public GraphTraversal2 max()
        {
            AddGremlinOperator(new GremlinMaxOp());
            return this;
        }

        public GraphTraversal2 max(GremlinKeyword.Scope scope)
        {
            AddGremlinOperator(new GremlinMaxOp(scope));
            return this;
        }

        public GraphTraversal2 mean()
        {
            AddGremlinOperator(new GremlinMeanOp());
            return this;
        }

        public GraphTraversal2 mean(GremlinKeyword.Scope scope)
        {
            AddGremlinOperator(new GremlinMeanOp(scope));
            return this;
        }

        public GraphTraversal2 min()
        {
            AddGremlinOperator(new GremlinMinOp());
            return this;
        }

        public GraphTraversal2 min(GremlinKeyword.Scope scope)
        {
            AddGremlinOperator(new GremlinMinOp(scope));
            return this;
        }


        public GraphTraversal2 not(GraphTraversal2 notTraversal)
        {
           AddGremlinOperator(new GremlinNotOp(notTraversal));
            return this;
        }
        public GraphTraversal2 option(object pickToken, GraphTraversal2 traversalOption)
        {
            if (LastGremlinTranslationOp is GremlinChooseOp)
            {
                (LastGremlinTranslationOp as GremlinChooseOp).OptionDict[pickToken] = traversalOption;
                return this;
            }
            else
            {
                throw new Exception("Option step only can follow by choose step.");
            }
        }
        //public GraphTraversal2 option(Traversal<E, E2 tarversalOption>

        public GraphTraversal2 optional(GraphTraversal2 traversalOption)
        {
            AddGremlinOperator(new GremlinOptionalOp(traversalOption));
            return this;
        }

        public GraphTraversal2 Or(params GraphTraversal2[] orTraversals)
        {
            AddGremlinOperator(new GremlinOrOp(orTraversals));
            return this;
        }

        public GraphTraversal2 order()
        {
            AddGremlinOperator(new GremlinOrderOp());
            return this;
        }

        //public GraphTraversal2 order(Scope scope)

        public GraphTraversal2 otherV()
        {
            AddGremlinOperator(new GremlinOtherVOp());
            return this;
        }

        public GraphTraversal2 Out(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinOutOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 outE(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinOutEOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 outV()
        {
            AddGremlinOperator(new GremlinOutVOp());
            return this;
        }

        //public GraphTraversal2 pageRank()
        //public GraphTraversal2 pageRank(double alpha)
        //public GraphTraversal2 path()
        //public GraphTraversal2 peerPressure()
        //public GraphTraversal2 profile()
        //public GraphTraversal2 profile(string sideEffectKey)
        //public GraphTraversal2 program(VertexProgram<?> vertexProgram)

        public GraphTraversal2 project(params string[] projectKeys)
        {
            AddGremlinOperator(new GremlinProjectOp(projectKeys));
            return this;
        }

        public GraphTraversal2 properties(params string[] propertyKeys)
        {
            AddGremlinOperator(new GremlinPropertiesOp(propertyKeys));
            return this;
        }

        public GraphTraversal2 property(params object[] keyValues)
        {
            AddGremlinOperator(new GremlinPropertyOp(keyValues));
            return this;
        }

        //public GraphTraversal2 property(VertexProperty.Cardinality cardinality, string key, string value, params string[] keyValues)

        public GraphTraversal2 propertyMap(params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 range(int low, int high)
        {
            AddGremlinOperator(new GremlinRangeOp(low, high));
            return this;
        }

        public GraphTraversal2 repeat(GraphTraversal2 repeatTraversal)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).RepeatTraversal = repeatTraversal;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp(repeatTraversal));
            }
            return this;
        }

        //public GraphTraversal2 sack() //Deprecated
        //public GraphTraversal2 sack(BiFunction<V, U, V>) sackOperator) //Deprecated
        //public GraphTraversal2 sack(BiFunction<V, U, V>) sackOperator, string, elementPropertyKey) //Deprecated

        public GraphTraversal2 sample(int amountToSample)
        {
            AddGremlinOperator(new GremlinSampleOp(amountToSample));
            return this;
        }

        //public GraphTraversal2 sample(Scope scope, int amountToSample)
        //public GraphTraversal2 select(Column column)
        //public GraphTraversal2 select(Pop pop, string selectKey)

        public GraphTraversal2 select(GremlinKeyword.Pop pop, params string[] selectKeys)
        {
            AddGremlinOperator(new GremlinSelectOp(pop, selectKeys));
            return this;
        }
        //public GraphTraversal2 select(string selectKey)
        public GraphTraversal2 select(params string[] selectKeys)
        {
            AddGremlinOperator(new GremlinSelectOp(selectKeys));
            return this;
        }

        //public GraphTraversal2 sideEffect(Consumer<Traverser<E>> consumer)

        public GraphTraversal2 sideEffect(GraphTraversal2 sideEffectTraversal)
        {
            AddGremlinOperator(new GremlinSideEffectOp(sideEffectTraversal));
            return this;    
        }

        //public GraphTraversal2 simplePath()
        //public GraphTraversal2 store(string sideEffectKey)
        //public GraphTraversal2 subgraph(string sideEffectKey)

        public GraphTraversal2 sum()
        {
            AddGremlinOperator(new GremlinSumOp());
            return this;
        }

        //public GraphTraversal2 sum(Scope scope)

        public GraphTraversal2 tail()
        {
            AddGremlinOperator(new GremlinTailOp());
            return this;
        }

        public GraphTraversal2 tail(long limit)
        {
            AddGremlinOperator(new GremlinTailOp(limit));
            return this;
        }

        //public GraphTraversal2 tail(Scope scope)
        //public GraphTraversal2 tail(Scope scope, long limit)

        public GraphTraversal2 timeLimit(long timeLimit)
        {
            throw new NotImplementedException();
            return this;
        }

        public GraphTraversal2 times(int maxLoops)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).RepeatTimes = maxLoops;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).RepeatTimes = maxLoops;
            }
            return this;
        }

        //public GraphTraversal2 to(Direction direction, params string[] edgeLabels)

        public GraphTraversal2 to(string toGremlinTranslationOperatorLabel)
        {
            AddGremlinOperator(new GremlinToOp(toGremlinTranslationOperatorLabel));
            return this;
        }

        public GraphTraversal2 to(GraphTraversal2 toVertex)
        {
            AddGremlinOperator(new GremlinToOp(toVertex));
            return this;
        }

        //public GraphTraversal2 toE(Direction direction, params string[] edgeLabels)
        //public GraphTraversal2 toV(Direction direction)

        public GraphTraversal2 tree()
        {
            AddGremlinOperator(new GremlinTreeOp());
            return this;
        }
        //public GraphTraversal2 tree(string sideEffectKey)

        public GraphTraversal2 unfold()
        {
            AddGremlinOperator(new GremlinUnfoldOp());
            return this;
        }

        public GraphTraversal2 union(params GraphTraversal2[] unionTraversals)
        {
            AddGremlinOperator(new GremlinUnionOp(unionTraversals));
            return this;
        }

        public GraphTraversal2 until(Predicate untilPredicate)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).TerminationPredicate = untilPredicate;
                (GetEndOp() as GremlinRepeatOp).StartFromContext = true;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).TerminationPredicate = untilPredicate;
            }
            return this;
        }

        public GraphTraversal2 until(GraphTraversal2 untilTraversal)
        {
            if (GetEndOp() is GremlinRepeatOp)
            {
                (GetEndOp() as GremlinRepeatOp).TerminationTraversal = untilTraversal;
                (GetEndOp() as GremlinRepeatOp).StartFromContext = true;
            }
            else
            {
                AddGremlinOperator(new GremlinRepeatOp());
                (GetEndOp() as GremlinRepeatOp).TerminationTraversal = untilTraversal;
            }
            return this;
        }

        public GraphTraversal2 V(params object[] vertexIdsOrElements)
        {
            AddGremlinOperator(new GremlinVOp(vertexIdsOrElements));
            return this;
        }

        public GraphTraversal2 V(List<object> vertexIdsOrElements)
        {
            AddGremlinOperator(new GremlinVOp(vertexIdsOrElements));
            return this;
        }

        public GraphTraversal2 value()
        {
            AddGremlinOperator(new GremlinValueOp());
            return this;
        }

        public GraphTraversal2 valueMap(Boolean includeTokens, params string[] propertyKeys)
        {
            throw new NotImplementedException();
            return this;
        }

        public GraphTraversal2 valueMap(params string[] propertyKeys)
        {
            throw new NotImplementedException();
            return this;
        }

        public GraphTraversal2 values(params string[] propertyKeys)
        {
            AddGremlinOperator(new GremlinValuesOp(propertyKeys));
            return this;
        }

        public GraphTraversal2 where(Predicate predicate)
        {
            AddGremlinOperator(new GremlinWhereOp(predicate));
            return this;
        }

        public GraphTraversal2 where(string startKey, Predicate predicate)
        {
            AddGremlinOperator(new GremlinWhereOp(startKey, predicate));
            return this;
        }

        public GraphTraversal2 where(GraphTraversal2 whereTraversal)
        {
            AddGremlinOperator(new GremlinWhereOp(whereTraversal));
            return this;
        }

        public static GraphTraversal2 __()
        {
            GraphTraversal2 newGraphTraversal = new GraphTraversal2();
            newGraphTraversal.AddGremlinOperator(new GremlinParentContextOp());
            return newGraphTraversal;
        }

        public static GraphTraversal2 g()
        {
            return new GraphTraversal2(); ;
        }

        public List<object> toList()
        {
            //TODO
            var Params = LastGremlinTranslationOp.ToSqlScript().ToString();
            return new List<object>() {1};
        }
    }
}


